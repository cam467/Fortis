namespace KnowBe4.Core.Services
{
    using System.Collections.Generic;
    using KnowBe4.Core.Models;
    using KnowBe4.Core.Entities;

    public interface IKronosApi
    {
        KronosFoundResults GetUsers(List<User> users);
    }
}