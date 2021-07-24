namespace KnowBe4.Core.Services
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;

    public interface IKnowBe4Service
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