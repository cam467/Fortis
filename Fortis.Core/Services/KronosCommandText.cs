namespace KnowBe4.Core.Services
{

    public class KronosCommandText : IKronosCommandText
    {
        public string SelectUserById => "select employeeid employee_number, firstname first_name, lastname last_name, personemail email, supervisorname manager_name, supervisoremail manager_email from bucdata_dev.dbo.lf_employees_active_all_with_supers where employeeid = @employeenumber;";
        public string SelectUserByFirstLastName => "select employeeid employee_number, firstname first_name, lastname last_name, personemail email, supervisorname manager_name, supervisoremail manager_email from bucdata_dev.dbo.lf_employees_active_all_with_supers where lastname like @lastname and firstname like @firstname;";
        public string SelectUserByLastNameEmail => "select employeeid employee_number, firstname first_name, lastname last_name, personemail email, supervisorname manager_name, supervisoremail manager_email from bucdata_dev.dbo.lf_employees_active_all_with_supers where lastname like @lastname and personallemails like @email;";
    }
}