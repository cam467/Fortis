namespace Fortis.Core.Models
{
    public class SecretServerLoginCreds
    {
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }

        public SecretServerLoginCreds()
        {
            grant_type = "password";
        }
    }
}