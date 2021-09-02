namespace Fortis.Core.Models
{
    using CsvHelper.Configuration;
    using Fortis.Core.Entities;

    public class UserMap : ClassMap<User>
	{
		public UserMap()
		{
			Map(m => m.email).Name("Email");
			Map(m => m.first_name).Name("First Name");
			Map(m => m.last_name).Name("Last Name");
			Map(m => m.phone_number).Name("Phone Number");
			Map(m => m.employee_number).Name("Employee Number");
			Map(m => m.password).Name("Password");
			Map(m => m.adi_manageable).Name("AD Managed");
			Map(m => m.employee_start_date).Name("Employee Start Date");
			Map(m => m.manager_name).Name("Manager Name");
			Map(m => m.manager_email).Name("Manager Email");
		}
	}
}