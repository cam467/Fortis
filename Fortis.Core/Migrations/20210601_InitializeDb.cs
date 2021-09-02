namespace Fortis.Core.Migrations
{
    using FluentMigrator;

    [Migration(20210402021400)]
    [Tags("data")]
    public class IntializeDB : Migration
    {
        public override void Up()
        {
            Create.Table("accounts")
                .WithColumn("name").AsString().NotNullable().PrimaryKey("namekey")
                .WithColumn("type").AsString(30).Nullable()
                .WithColumn("subscription_level").AsString(30).Nullable()
                .WithColumn("subscription_end_date").AsDateTime2().Nullable()
                .WithColumn("number_of_seats").AsInt32().Nullable()
                .WithColumn("current_risk_score").AsDecimal(15,2).Nullable();

            Create.Table("account_domains")
                .WithColumn("name").AsString(30).NotNullable().PrimaryKey()
                .WithColumn("domain_name").AsString(100).NotNullable().PrimaryKey();
            
            // Create.PrimaryKey("accountdomainsprikey").OnTable("account_domains").Columns(new string[] {"name", "domain_name"});

            Create.Table("campaigns")
                .WithColumn("campaign_id").AsInt32().NotNullable().PrimaryKey("campaignidkey")
                .WithColumn("name").AsString(30).NotNullable()
                .WithColumn("status").AsString(30).Nullable()
                .WithColumn("duration_type").AsString(30).Nullable()
                .WithColumn("start_date").AsDateTime2().Nullable()
                .WithColumn("end_date").AsDateTime2().Nullable()
                .WithColumn("relative_duration").AsString(30).Nullable()
                .WithColumn("auto_enroll").AsBoolean().Nullable()
                .WithColumn("allow_multiple_enrollments").AsBoolean().Nullable();

            Create.Table("enrollments")
                .WithColumn("enrollment_id").AsInt32().NotNullable().PrimaryKey("enrollments_prikey")
                .WithColumn("content_type").AsString(30).Nullable()
                .WithColumn("module_name").AsString(100).Nullable()
                .WithColumn("campaign_name").AsString(30).Nullable()
                .WithColumn("enrollment_date").AsDateTime2().Nullable()
                .WithColumn("start_date").AsDateTime2().Nullable()
                .WithColumn("completion_date").AsDateTime2().Nullable()
                .WithColumn("status").AsString(30).Nullable()
                .WithColumn("time_spent").AsInt32().Nullable()
                .WithColumn("policy_acknowledged").AsBoolean().Nullable();

            Create.Table("users")
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey("users_useridprikey")
                .WithColumn("employee_number").AsString(30).Nullable()
                .WithColumn("first_name").AsString(30).NotNullable()
                .WithColumn("last_name").AsString(30).Nullable()
                .WithColumn("job_title").AsString(30).Nullable()
                .WithColumn("email").AsString(100).Nullable()
                .WithColumn("phish_prone_percentage").AsDecimal(15,2).NotNullable()
                .WithColumn("phone_number").AsString(30).Nullable()
                .WithColumn("location").AsString(30).Nullable()
                .WithColumn("division").AsString(30).Nullable()
                .WithColumn("manager_name").AsString(60).Nullable()
                .WithColumn("manager_email").AsString(100).Nullable()
                .WithColumn("adi_manageable").AsBoolean().Nullable()
                .WithColumn("adi_guid").AsString(100).Nullable()
                .WithColumn("joined_on").AsDateTime2().Nullable()
                .WithColumn("last_sign_in").AsDateTime2().Nullable()
                .WithColumn("status").AsString(30).Nullable()
                .WithColumn("organization").AsString(30).Nullable()
                .WithColumn("department").AsString(30).Nullable()
                .WithColumn("employee_start_date").AsDateTime2().Nullable()
                .WithColumn("current_risk_score").AsDecimal(15,2).Nullable();

            Create.Table("groups")
                .WithColumn("group_id").AsInt32().NotNullable().PrimaryKey("groups_groupidprikey")
                .WithColumn("name").AsString(30).NotNullable()
                .WithColumn("group_type").AsString(20).Nullable()
                .WithColumn("adi_guid").AsString(100).Nullable()
                .WithColumn("member_count").AsInt32().Nullable()
                .WithColumn("current_risk_score").AsDecimal(15,2).Nullable()
                .WithColumn("status").AsString(30).Nullable();

            IfDatabase("sqlserver").Create.Table("riskscores")
                .WithColumn("riskscore_id").AsInt32().NotNullable().Identity()
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("score_type").AsString(3).NotNullable().PrimaryKey()
                .WithColumn("score_id").AsString(20).NotNullable()
                .WithColumn("score_date").AsDateTime2().NotNullable().PrimaryKey()
                .WithColumn("risk_score").AsDecimal(15,2).Nullable();

            IfDatabase("sqlite").Create.Table("riskscores")
                .WithColumn("riskscore_id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("id").AsInt32().NotNullable()
                .WithColumn("score_type").AsString(3).Nullable()
                .WithColumn("score_id").AsString(20).Nullable()
                .WithColumn("score_date").AsDateTime2().NotNullable()
                .WithColumn("risk_score").AsDecimal(15,2).Nullable();

            // Create.PrimaryKey("riskscores_prikey").OnTable("riskscores").Columns(new string[] {"score_type","score_id","score_date"});

            // Create.ForeignKey("riskscores_forkeyuser").FromTable("users").ForeignColumn("user_id").ToTable("riskscores").PrimaryColumn("user_id");

            Create.Table("storepurchases")
                .WithColumn("store_purchase_id").AsInt32().NotNullable().PrimaryKey("storepurchases_prikey")
                .WithColumn("content_type").AsString(30).Nullable()
                .WithColumn("name").AsString(100).Nullable()
                .WithColumn("description").AsString(200).Nullable()
                .WithColumn("type").AsString(20).Nullable()
                .WithColumn("duration").AsInt32().Nullable()
                .WithColumn("retired").AsBoolean().Nullable()
                .WithColumn("retirement_date").AsDateTime2().Nullable()
                .WithColumn("publish_date").AsDateTime2().Nullable()
                .WithColumn("publisher").AsString(30).Nullable()
                .WithColumn("purchase_date").AsDateTime2().Nullable()
                .WithColumn("policy_url").AsString(200).Nullable();

            IfDatabase("sqlserver").Create.Table("campaign_content")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("store_purchase_id").AsInt32().NotNullable().PrimaryKey();

            IfDatabase("sqlite").Create.Table("campaign_content")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable()
                .WithColumn("store_purchase_id").AsInt32().NotNullable();

            // Create.PrimaryKey("campaigncontent_prikey").OnTable("campaign_content").Columns(new string[] {"campaign_id","store_purchase_id"});
            
            // Create.ForeignKey("campaigncontent_campaignidforkey").FromTable("campaigns")
            //     .ForeignColumn("campaign_id").ToTable("campaign_content").PrimaryColumn("campaign_id");
            // Create.ForeignKey("campaigncontent_storepurchaseidforkey").FromTable("storepurchases")
            //     .ForeignColumn("store_purchase_id").ToTable("campaign_content").PrimaryColumn("store_purchase_id");
            
            IfDatabase("sqlserver").Create.Table("campaign_groups")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("group_id").AsInt32().NotNullable().PrimaryKey();

            IfDatabase("sqlite").Create.Table("campaign_groups")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable()
                .WithColumn("group_id").AsInt32().NotNullable();

            // Create.PrimaryKey("campaigngroups_prikey").OnTable("campaign_groups").Columns(new string[] {"campaign_id","group_id"});

            // Create.ForeignKey("campaigngroups_campaignidforkey").FromTable("campaigns")
            //     .ForeignColumn("campaign_id").ToTable("campaign_groups").PrimaryColumn("campaign_id");
            // Create.ForeignKey("campaigngroups_groupidforkey").FromTable("groups")
            //     .ForeignColumn("group_id").ToTable("campaign_groups").PrimaryColumn("group_id");

            IfDatabase("sqlserver").Create.Table("campaign_modules")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("store_purchase_id").AsInt32().NotNullable().PrimaryKey();

            IfDatabase("sqlite").Create.Table("campaign_modules")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("campaign_id").AsInt32().NotNullable()
                .WithColumn("store_purchase_id").AsInt32().NotNullable();

            // Create.PrimaryKey("campaignmodules_prikey").OnTable("campaign_modules").Columns(new string[] {"campaign_id","store_purchase_id"});
            
            // Create.ForeignKey("campaignmodules_campaignidforkey").FromTable("campaigns")
            //     .ForeignColumn("campaign_id").ToTable("campaign_modules").PrimaryColumn("campaign_id");
            // Create.ForeignKey("campaignmodules_storepurchaseidforkey").FromTable("storepurchases")
            //     .ForeignColumn("store_purchase_id").ToTable("campaign_modules").PrimaryColumn("store_purchase_id");

            IfDatabase("sqlserver").Create.Table("enrollment_user")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("enrollment_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey();

            IfDatabase("sqlite").Create.Table("enrollment_user")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("enrollment_id").AsInt32().NotNullable()
                .WithColumn("user_id").AsInt32().NotNullable();

            // Create.PrimaryKey("enrollmentuser_prikey").OnTable("enrollment_user").Columns(new string[] {"enrollment_id","user_id"});
            
            // Create.ForeignKey("enrollmentuser_enrollmentidforkey").FromTable("enrollments")
            //     .ForeignColumn("enrollment_id").ToTable("enrollment_user").PrimaryColumn("enrollment_id");
            // Create.ForeignKey("enrollmentuser_useridforkey").FromTable("users")
            //     .ForeignColumn("user_id").ToTable("enrollment_user").PrimaryColumn("user_id");
            IfDatabase("sqlserver").Create.Table("user_groups")
                .WithColumn("id").AsInt32().NotNullable().Identity()
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("group_id").AsInt32().NotNullable().PrimaryKey();

            IfDatabase("sqlite").Create.Table("user_groups")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().NotNullable()
                .WithColumn("group_id").AsInt32().NotNullable();

            // Create.PrimaryKey("usergroups_prikey").OnTable("user_groups").Columns(new string[] {"user_id","group_id"});
            
            // Create.ForeignKey("usergroups_useridforkey").FromTable("users")
            //     .ForeignColumn("user_id").ToTable("user_groups").PrimaryColumn("user_id");
            // Create.ForeignKey("usergroups_groupidforkey").FromTable("groups")
            //     .ForeignColumn("group_id").ToTable("user_groups").PrimaryColumn("group_id");
        }

        public override void Down()
        {
            Delete.ForeignKey("enrollmentuser_enrollmentidforkey");
            Delete.ForeignKey("enrollmentuser_useridforkey");
            Delete.Table("enrollment_user");
            Delete.ForeignKey("campaigncontent_campaignidforkey");
            Delete.ForeignKey("campaigncontent_storepurchaseidforkey");
            Delete.Table("campaign_content");
            Delete.ForeignKey("campaigngroups_campaignidforkey");
            Delete.ForeignKey("campaigngroups_groupidforkey");
            Delete.Table("campaign_groups");
            Delete.ForeignKey("campaignmodules_campaignidforkey");
            Delete.ForeignKey("campaignmodules_storepurchaseidforkey");
            Delete.Table("campaign_modules");
            Delete.ForeignKey("usergroups_useridforkey");
            Delete.ForeignKey("usergroups_groupidforkey");
            Delete.Table("user_groups");
            Delete.Table("accounts");
            Delete.Table("account_domains");
            Delete.Table("campaigns");
            Delete.Table("enrollments");
            Delete.Table("users");
            Delete.Table("groups");
            Delete.Table("riskscores");
            Delete.Table("storepurchases");
        }
    }
}