namespace UKHO.S100PermitService.Common.Security
{
    public class S100Manufacturer : IS100Manufacturer
    {
        private readonly string _id;
        private readonly string _key;

        public S100Manufacturer(string id, string key)
        {
            _id = id;
            _key = key;
        }

        public string Decrypt(string encrypted)
        {
            S100Crypt c = new(_key);
            return BitConverter.ToString(c.Decrypt(encrypted)).Replace("-", "");
        }

        public string Encrypt(string unencrypted)
        {
            S100Crypt c = new(_key);
            return BitConverter.ToString(c.Encrypt(unencrypted)).Replace("-", "");
        }
    }
}
