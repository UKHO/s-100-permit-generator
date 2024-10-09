﻿using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using UKHO.S100PermitService.Common.Clients;
using UKHO.S100PermitService.Common.Events;
using UKHO.S100PermitService.Common.Exceptions;
using UKHO.S100PermitService.Common.Providers;

namespace UKHO.S100PermitService.Common.Services
{
    public class ManufacturerKeyService : IManufacturerKeyService
    {
        private readonly ILogger<ManufacturerKeyService> _logger;
        private readonly ICacheProvider _cacheProvider;
        private readonly ISecretClient _secretClient;

        public ManufacturerKeyService(ILogger<ManufacturerKeyService> logger,
                                      ICacheProvider cacheProvider,
                                      ISecretClient secretClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _secretClient = secretClient ?? throw new ArgumentNullException(nameof(secretClient));
        }

        public void CacheManufacturerKeys()
        {
            _logger.LogInformation(EventIds.ManufacturerKeyCachingStart.ToEventId(), "Caching Of Manufacturer Keys started.");

            var secretProperties = _secretClient.GetPropertiesOfSecrets();
            if(!secretProperties.Any())
            {
                throw new PermitServiceException(EventIds.ManufacturerIdNotFoundInKeyVault.ToEventId(), "No Secrets found in Manufacturer Key Vault");
            }

            foreach(var secretProperty in secretProperties)
            {
                var secretName = secretProperty.Name;
                GetSetManufacturerValue(secretName);
            }

            _logger.LogInformation(EventIds.ManufacturerKeyCachingEnd.ToEventId(), "Caching Of Manufacturer Keys End.");
        }

        public string GetManufacturerKeys(string secretName)
        {
            try
            {
                var secretValue = _cacheProvider.GetCacheKey(secretName);
                if(string.IsNullOrEmpty(secretValue))
                {
                    var secret = GetSetManufacturerValue(secretName);
                    return secret.Value;
                }
                return secretValue;
            }
            catch(Exception ex)
            {
                throw new PermitServiceException(EventIds.ManufacturerIdNotFoundInKeyVault.ToEventId(), "No Secrets found in Manufacturer Key Vault, failed with Exception :{Message}", ex.Message);
            }
        }

        private KeyVaultSecret GetSetManufacturerValue(string secretName)
        {
            var secretValue = _secretClient.GetSecret(secretName);

            _cacheProvider.SetCacheKey(secretName, secretValue.Value);

            return secretValue;
        }
    }
}