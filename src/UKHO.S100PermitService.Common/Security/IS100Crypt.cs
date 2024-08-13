namespace UKHO.S100PermitService.Common.Security
{
    public interface IS100Crypt
    {
        byte[] Decrypt(string encrypted);
        byte[] Encrypt(byte[] unencrypted);
    }
}
