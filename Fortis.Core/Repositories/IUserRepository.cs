namespace KnowBe4.Core.Repositories
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;

    public interface IUserRepository
    {
        User GetByEmployeeNumber(int eid);
        User GetByID(int id);
        List<User> GetUsers();
        bool AddUsers(List<User> users);
    }
}