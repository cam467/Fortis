namespace Fortis.Core.Repositories
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public interface IUserRepository
    {
        User GetByEmployeeNumber(int eid);
        User GetByID(int id);
        List<User> GetUsers();
        bool AddUsers(List<User> users);
    }
}