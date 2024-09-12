using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using UKHO.S100PermitService.API.FunctionalTests.Configuration;
using UKHO.S100PermitService.API.FunctionalTests.Helpers;

namespace UKHO.S100PermitService.API.FunctionalTests.FunctionalTests
{
    public class PermitServiceTests : TestBase
    {
        private AuthTokenProvider _tokenProvider;
        private TokenConfiguration? _tokenConfiguration;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _tokenProvider = new AuthTokenProvider();
            var _serviceProvider = GetServiceProvider();
            _tokenConfiguration = _serviceProvider?.GetRequiredService<IOptions<TokenConfiguration>>().Value;
        }

        [Test]
        public async Task TestToCheckAuthentication()
        {
            var token = await _tokenProvider.GetPSToken(_tokenConfiguration!.ClientId!, _tokenConfiguration.ClientSecret!);
            Console.WriteLine(token);
        }

        [Test]
        public async Task TestToCheckNoAuthentication()
        {
            var token = await _tokenProvider.GetPSToken(_tokenConfiguration!.ClientIdNoAuth!, _tokenConfiguration.ClientSecretNoAuth!);
            Console.WriteLine(token);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Cleanup();
        }
    }
}
