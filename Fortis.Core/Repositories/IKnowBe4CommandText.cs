namespace Fortis.Core.Repositories
{
    public interface IFortisCommandText
    {
        string InsertUsers { get; }
        string UpdateUsers { get; }
        string GetUsers { get; }
        string InsertUserGroups { get; }
        string InsertGroups { get; }
        string UpdateGroups { get; }
        string InsertRiskScores { get; }
        string InsertStorePurchases { get; }
        string UpdateStorePurchases { get; }
        string InsertEnrollments { get; }
        string UpdateEnrollments { get; }
        string InsertEnrollmentUser { get; }
        string UpdateEnrollmentUser { get; }
        string InsertCampaigns { get; }
        string UpdateCampaigns { get; }
        string InsertCampaignGroups { get; }
        string UpdateCampaignGroups { get; }
        string InsertCampaignModules { get; }
        string UpdateCampaignModules { get; }
        string InsertCampaignContent { get; }
        string UpdateCampaignContent { get; }
        string InsertAccount { get; }
        string UpdateAccount { get; }
        string InsertAccountDomains { get; }
        string UpdateAccountDomains { get; }
    }
}