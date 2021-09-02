namespace Fortis.Core.Repositories
{

    public class FortisCommandText : IFortisCommandText
    {
        public string InsertUsers => "insert into users (id,employee_number,first_name,last_name,job_title,email,phish_prone_percentage,phone_number,location,division,manager_name,manager_email,adi_manageable,adi_guid,joined_on,last_sign_in,status,organization,department,employee_start_date,current_risk_score) values (@id,@employee_number,@first_name,@last_name,@job_title,@email,@phish_prone_percentage,@phone_number,@location,@division,@manager_name,@manager_email,@adi_manageable,@adi_guid,@joined_on,@last_sign_in,@status,@organization,@department,@employee_start_date,@current_risk_score);";
        public string UpdateUsers => "update users set employee_number = @employee_number,first_name = @first_name,last_name = @last_name,job_title = @job_title,email = @email,phish_prone_percentage = @phish_prone_percentage,phone_number = @phone_number,location = @location,division = @division,manager_name = @manager_name,manager_email = @manager_email,adi_manageable = @adi_manageable,adi_guid = @adi_guid,joined_on = @joined_on,last_sign_in = @last_sign_in,status = @status,organization = @organization,department = @department,employee_start_date = @employee_start_date,current_risk_score = @current_risk_score where id = @id;";
        public string GetUsers => "select id, employee_number, first_name, last_name, email, status from users;";
        public string InsertUserGroups => "insert into user_groups (user_id,group_id) values (@user_id,@group_id);";
        public string InsertGroups => "insert into groups (id,name,group_type,adi_guid,member_count,current_risk_score,status) values (@id,@name,@group_type,@adi_guid,@member_count,@current_risk_score,@status);";
        public string UpdateGroups => "update groups set name = @name,group_type = @group_type,adi_guid = @adi_guid,member_count = @member_count,current_risk_score = @current_risk_score,status = @status where id = @id;";
        public string InsertRiskScores => "insert into riskscores(score_type,score_id,risk_score,score_date) values (@score_type,@score_id,@risk_score,@score_date);";
        public string InsertStorePurchases => "insert into storepurchases (store_purchase_id,content_type,name,description,type,duration,retired,retirement_date,publish_date,publisher,purchase_date,policy_url) values (@store_purchase_id,@content_type,@name,@description,@type,@duration,@retired,@retirement_date,@publish_date,@publisher,@purchase_date,@policy_url);";
        public string UpdateStorePurchases => "update storepurchases set content_type = @content_type,name = @name,description = @description,type = @type,duration = @duration,retired = @retired,retirement_date = @retirement_date,publish_date = @publish_date,publisher = @publisher,purchase_date = @purchase_date,policy_url = @policy_url where store_purchase_id = @store_purchase_id;";
        public string InsertEnrollments => "insert into enrollments values (@enrollment_id,@content_type,@module_name,@campaign_name,@enrollment_date,@start_date,@completion_date,@status,@time_spent,@policy_acknowledged);";
        public string UpdateEnrollments => "update enrollments set content_type = @content_type,module_name = @module_name,campaign_name = @campaign_name, enrollment_date = @enrollment_date, start_date = @start_date,completion_date = @completion_date,status = @status,time_spent = @time_spent,policy_acknowledged = @policy_acknowledged where enrollment_id = @enrollment_id;";
        public string InsertEnrollmentUser => "insert into enrollment_user values (@enrollment_id,@user_id);";
        public string UpdateEnrollmentUser => "update enrollment_user set user_id = @user_id where enrollment_id = @enrollment_id;";
        public string InsertCampaigns => "insert into campaigns values (@campaign_id,@name,@status,@duration_type,@start_date,@end_date,@relative_duration,@auto_enroll,@allow_multiple_enrollments);";
        public string UpdateCampaigns => "update campaigns set name = @name,status = @status,duration_type = @duration_type, start_date = @start_date, end_date = @end_date,relative_duration = @relative_duration,auto_enroll = @auto_enroll,allow_multiple_enrollments = @allow_multiple_enrollments where campaign_id = @campaign_id;";
        public string InsertCampaignGroups => "insert into campaign_groups values (@campaign_id,@group_id);";
        public string UpdateCampaignGroups => "update campaign_groups set group_id = @group_id where campaign_id = @campaign_id;";
        public string InsertCampaignModules => "insert into campaign_modules values (@campaign_id,@store_purchase_id);";
        public string UpdateCampaignModules => "update campaign_modules set store_purchase_id = @store_purchase_id where campaign_id = @store_purchase_id;";
        public string InsertCampaignContent => "insert into campaign_content values (@campaign_id,@store_purchase_id);";
        public string UpdateCampaignContent => "update campaign_content set store_purchase_id = @store_purchase_id where campaign_id = @store_purchase_id;";
        public string InsertAccount => "insert into accounts values (@name,@type,@subscription_level,@subscription_end_date,@number_of_seats,@current_risk_score);";
        public string UpdateAccount => "update accounts set type = @type,subscription_level = @subscription_level, subscription_end_date = @subscription_end_date, number_of_seats = @number_of_seats,current_risk_score = @current_risk_score where name = @name;";
        public string InsertAccountDomains => "insert into account_domains values (@name,@domain_name);";
        public string UpdateAccountDomains => "update account_domains set domain_name = @domain_name where name = @name;";
    }
}