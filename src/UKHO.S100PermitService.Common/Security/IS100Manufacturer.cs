namespace UKHO.S100PermitService.Common.Security
{
    public interface IS100Manufacturer
    {
        string Decrypt(string encrypted);
        string Encrypt(string unencrypted);
    }
}
