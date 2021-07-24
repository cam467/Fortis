namespace KnowBe4.Core.Services
{
    using KnowBe4.Core.Models;
    using System.Collections.Generic;

    public interface IPasswordRepository
    {
        string TestService();
        SecretServerSecret GetSecret(int id);
        string GetSecretUsername(int id);
        string GetSecretPassword(int id);
        List<SecretServerSecretSummary> GetSecrets();
    }
}