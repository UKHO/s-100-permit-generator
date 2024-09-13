using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;

namespace UKHO.S100PermitService.Common.Cache
{
    public class ManufactureCache : IManufactureCache
    {
        private readonly TimeSpan _cacheTimeout;
        private readonly IMemoryCache _memoryCache;
        private readonly SecretClient _secretClient;

        public ManufactureCache(TimeSpan cacheTimeout, IMemoryCache memoryCache, SecretClient secretClient)
        {
            _cacheTimeout = cacheTimeout;
            _memoryCache = memoryCache;
            _secretClient = secretClient;
            CacheAllManufacture();
        }

        public void CacheAllManufacture()
        {
            try
            {
                //Get the keys of all the existing secrets
                var secretProperties = _secretClient.GetPropertiesOfSecrets();
                foreach(var secretProperty in secretProperties)
                {
                    var mId = secretProperty.Name;
                    GetManufactureKey(mId, false);
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public string GetManufactureKey(string manufactureId)
        {
            return GetManufactureKey(manufactureId, false);

        }

        public string GetManufactureKey(string manufactureId, bool ignoreCache)
        {
            // Check if the secret is already cached, return it if it is
            if(ignoreCache == false && _memoryCache.TryGetValue(manufactureId, out string? mKey))
            {
                return mKey;
            }

            // Fetch latest secret from Key Vault
            var manufactureKey = _secretClient.GetSecret(manufactureId).Value.Value;

            // Store found secret in memory cache
            _memoryCache.Set(manufactureId, manufactureKey, _cacheTimeout);

            return manufactureKey;
        }
    }
}
