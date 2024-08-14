using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UKHO.S100PermitService.Common.Helpers;

namespace UKHO.S100PermitService.Common.Security
{
    public class S100DataPermit : IS100DataPermit
    {
        private readonly string fileName;
        private readonly int edtn;
        private readonly DateTime permitEndDate;
        private readonly string encryptedDataKey;
        private readonly S100ProductSpecification productSpecification;

        public const string PERMIT_ELEMENT = "permit";
        public const string FILENAME_ELEMENT = "filename";
        public const string EDITION_NUMBER_ELEMENT = "editionNumber";
        public const string EXPIRY_ELEMENT = "expiry";
        public const string ENCRYPTED_KEY_ELEMENT = "encryptedKey";

        public const string EXPIRY_DATE_FORMAT = "yyyyMMdd";

        public S100DataPermit(string fileName, int edtn, DateTime permitEndDate, 
            S100ProductSpecification productSpecification, string encryptedDataKey = "")
        {
            this.fileName = fileName;
            this.edtn = edtn;
            this.permitEndDate = permitEndDate;
            this.encryptedDataKey = encryptedDataKey;
            this.productSpecification = productSpecification;
        }

        public S100DataPermit Create(string dataKey, string hwId)
        {
            S100Crypt crypt = new(hwId);
            string encryptedDataKey = Hex.ToString(crypt.Encrypt(Hex.FromString(dataKey)));
            return new S100DataPermit(fileName, edtn, permitEndDate, productSpecification, encryptedDataKey);
        }

        public string GetEncryptedDataKey()
        {
            return encryptedDataKey;
        }

    }
}
