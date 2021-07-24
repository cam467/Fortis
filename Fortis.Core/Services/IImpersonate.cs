namespace KnowBe4.Core.Services
{
    using System;

    public interface IImpersonate
    {
        T RunAsImpersonated<T>(Func<T> func, string username, string password, string domain);
    }
}