﻿namespace UKHO.S100PermitService.API.FunctionalTests.Helpers
{
    public static class PSEndPointHelper
    {
        static readonly HttpClient _httpClient = new();
        private static string? _uri;

        public static async Task <HttpResponseMessage> PermitServiceEndPoint(string? baseUrl, string? accessToken, int licenceId)
        {
            _uri = $"{baseUrl}/permits/{licenceId}";
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, _uri) ;
            if(!string.IsNullOrEmpty(accessToken))
            {
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            return await _httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
        }
    }
}
