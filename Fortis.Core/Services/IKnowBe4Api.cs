namespace KnowBe4.Core.Services
{
    using KnowBe4.Core.Entities;
    using KnowBe4.Core.Models;
    using System.Collections.Generic;

    public interface IKnowBe4Api
    {
        bool UploadUserData(byte[] usersfile);
        Account GetAccount();
        List<User> GetUsers();
        User GetUser(int id);
        List<StorePurchase> GetStorePurchases();
        StorePurchase GetStorePurchase(int id);
        List<KGroup> GetGroups();
        List<Campaign> GetCampaigns();
        Campaign GetCampaign(long id);
        List<Enrollment> GetEnrollments();
        Enrollment GetEnrollment(long id);
        bool AddUsers(List<User> users);
        bool ArchiveUsers(List<User> users);
        bool UpdateUsers(List<User> users);
    }
}