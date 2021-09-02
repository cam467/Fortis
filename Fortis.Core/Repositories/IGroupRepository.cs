namespace Fortis.Core.Repositories
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public interface IGroupRepository
    {
        bool AddGroups(List<KGroup> groups);
        KGroup GetByID(int id);
        KGroup GetByName(int name);
        List<KGroup> GetGroups();
    }
}