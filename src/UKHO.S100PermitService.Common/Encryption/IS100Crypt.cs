﻿using UKHO.S100PermitService.Common.Models.ProductKeyService;

namespace UKHO.S100PermitService.Common.Encryption
{
    public interface IS100Crypt
    {
        IEnumerable<ProductKey> GetEncKeysFromProductKeys(IEnumerable<ProductKeyServiceResponse> productKeyServiceResponses, string hardwareId);
    }
}