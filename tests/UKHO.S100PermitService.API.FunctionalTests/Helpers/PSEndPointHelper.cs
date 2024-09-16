using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKHO.S100PermitService.API.FunctionalTests.Helpers
{
    public static class PSEndPointHelper
    {
        static readonly HttpClient _httpClient = new();
        private static string? uri;

        public static async Task <HttpResponseMessage> PermitServiceEndPoint(string? baseUrl, string? accessToken, string licenceId)
        {
            uri = $"{baseUrl}/permits/{licenceId}";
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri) ;
            if(!string.IsNullOrEmpty(accessToken))
            {
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            return await _httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
        }

    }
}
