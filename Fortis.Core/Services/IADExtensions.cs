namespace Fortis.Core.Services
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public interface IADExtensions
    {
        List<User> GetAllADUsersForGroups(List<string> _groups, string _ouroot);
    }
}