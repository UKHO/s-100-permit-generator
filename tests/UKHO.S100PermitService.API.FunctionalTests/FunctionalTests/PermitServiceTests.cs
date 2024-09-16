using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Net;
using UKHO.S100PermitService.API.FunctionalTests.Configuration;
using UKHO.S100PermitService.API.FunctionalTests.Helpers;
using FluentAssertions;

namespace UKHO.S100PermitService.API.FunctionalTests.FunctionalTests
{
    public class PermitServiceTests : TestBase
    {
        private AuthTokenProvider _tokenProvider;
        private TokenConfiguration? _tokenConfiguration;
        private PermitServiceApiConfiguration _apiConfiguration;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _tokenProvider = new AuthTokenProvider();
            var _serviceProvider = GetServiceProvider();
            _tokenConfiguration = _serviceProvider?.GetRequiredService<IOptions<TokenConfiguration>>().Value;
            _apiConfiguration = _serviceProvider?.GetRequiredService<IOptions<PermitServiceApiConfiguration>>().Value;
        }

        [Test]
        public async Task TestToResponseForValidUserWithRole()
        {
            var token = await _tokenProvider.GetPSToken(_tokenConfiguration!.ClientId!, _tokenConfiguration.ClientSecret!);
            var response = await PSEndPointHelper.PermitServiceEndPoint(_apiConfiguration!.BaseUrl, token, "2");
            response.StatusCode.Should().Be((HttpStatusCode)200);
        }

        [Test]
        public async Task TestToResponseForValidUserWithoutRole()
        {
            var token = await _tokenProvider.GetPSToken(_tokenConfiguration!.ClientIdNoAuth!, _tokenConfiguration.ClientSecretNoAuth!);
            var response = await PSEndPointHelper.PermitServiceEndPoint(_apiConfiguration!.BaseUrl, token, "1");
            response.StatusCode.Should().Be((HttpStatusCode)403);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Cleanup();
        }
    }
}
