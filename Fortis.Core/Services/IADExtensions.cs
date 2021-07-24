namespace KnowBe4.Core.Services
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;

    public interface IADExtensions
    {
        List<User> GetAllADUsersForGroups(List<string> _groups, string _ouroot);
    }
}