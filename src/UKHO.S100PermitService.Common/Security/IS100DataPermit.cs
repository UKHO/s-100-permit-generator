namespace UKHO.S100PermitService.Common.Security
{
    public interface IS100DataPermit
    {
        public S100DataPermit CreateEncrypt(string dataKey, string hwId);
        public S100DataPermit CreateDecrypt(string dataKey, string hwId);
        public string GetDecryptedDataKey();
        string GetEncryptedDataKey();
    }
}
