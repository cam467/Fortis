namespace KnowBe4.Core.Services
{
    public interface ICrypto
    {
        byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV);
        string Encrypt(string clearText, string Password);
        byte[] Encrypt(byte[] clearData, string Password);
        void Encrypt(string fileIn, string fileOut, string Password);
        byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV);
        string Decrypt(string cipherText, string Password);
        byte[] Decrypt(byte[] cipherData, string Password);
        void Decrypt(string fileIn, string fileOut, string Password);
    }
}
