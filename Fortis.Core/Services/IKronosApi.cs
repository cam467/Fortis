namespace Fortis.Core.Services
{
    using System.Collections.Generic;
    using Fortis.Core.Models;
    using Fortis.Core.Entities;

    public interface IKronosApi
    {
        KronosFoundResults GetUsers(List<User> users);
    }
}