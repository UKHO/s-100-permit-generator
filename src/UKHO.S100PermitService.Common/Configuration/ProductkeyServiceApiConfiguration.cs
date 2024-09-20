﻿using System.Diagnostics.CodeAnalysis;

namespace UKHO.S100PermitService.Common.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProductkeyServiceApiConfiguration
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public int RequestTimeoutInMinutes { get; set; }
    }
}