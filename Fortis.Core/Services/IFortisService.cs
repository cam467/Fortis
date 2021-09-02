namespace Fortis.Core.Services
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public interface IFortisService
    {
        // bool AddAccount();
        // bool AddCampaigns();
        // bool AddEnrollments();
        // bool AddGroups();
        // bool AddStorePurchases();
        // bool AddUsers();
        List<User> GetAllADAccounts();
        List<User> GetUsers();
        List<KGroup> GetGroups();
        bool AddUsers();
        bool AddGroups();
        bool RunExport();
        bool RunImport();
        string TestConnection();
        bool SyncADUsersWithKBUsers();
    }
}