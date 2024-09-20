﻿namespace UKHO.S100PermitService.API.FunctionalTests.Configuration
{
    public class PermitServiceApiConfiguration
    {
        public string? BaseUrl { get; set; }
        public string? InvalidToken { get; set; }
        public int? ValidLicenceId { get; set; }
        public List<int>? InvalidLicenceIds { get; set; }
        public List<string>? NonIntegerLicenceIds { get; set; }
    }
}