namespace KnowBe4.Core.Repositories
{
    using System;

    public class UserCommandText : IUserCommandText
    {
        public string GetUsers => "Select user_id id, * from users";
        public string GetUserByID => "Select user_id id, * from users where user_id = @id";
        public string GetUserByEmployeeNumber => "Select user_id id, * from users where employee_number = @employee_number";
        public string InsertUsers => "insert into users (user_id,employee_number,first_name,last_name,job_title,email,phish_prone_percentage,phone_number,location,division,manager_name,manager_email,adi_manageable,adi_guid,joined_on,last_sign_in,status,organization,department,employee_start_date,current_risk_score) values (@user_id,@employee_number,@first_name,@last_name,@job_title,@email,@phish_prone_percentage,@phone_number,@location,@division,@manager_name,@manager_email,@adi_manageable,@adi_guid,@joined_on,@last_sign_in,@status,@organization,@department,@employee_start_date,@current_risk_score);";
        public string UpdateUsers => "update users set employee_number = @employee_number,first_name = @first_name,last_name = @last_name,job_title = @job_title,email = @email,phish_prone_percentage = @phish_prone_percentage,phone_number = @phone_number,location = @location,division = @division,manager_name = @manager_name,manager_email = @manager_email,adi_manageable = @adi_manageable,adi_guid = @adi_guid,joined_on = @joined_on,last_sign_in = @last_sign_in,status = @status,organization = @organization,department = @department,employee_start_date = @employee_start_date,current_risk_score = @current_risk_score where user_id = @user_id;";
    }
}