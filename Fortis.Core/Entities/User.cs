namespace KnowBe4.Core.Entities
{
    using System;
    using System.Collections.Generic;
    
    public class User
	{
		public int id {get;set;}
		public int user_id { get {return id;} }
		public string employee_number { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string job_title { get; set; }
		public string email { get; set; }
		public decimal phish_prone_percentage { get; set; }
		public string phone_number { get; set; }
		public string location { get; set; }
		public string division { get; set; }
		public string manager_name { get; set; }
		public string manager_email { get; set; }
		public string password { get; set; }
		public bool adi_manageable { get; set; }
		public string adi_guid { get; set; }
		public DateTime joined_on { get; set; }
		public DateTime? last_sign_in { get; set; }
		public string status { get; set; }
		public string organization { get; set; }
		public string department { get; set; }
        public List<int> groups { get; set; }
		public DateTime? employee_start_date { get; set; }
        public decimal current_risk_score { get; set; }
        public List<RiskScore> risk_score_history { get; set; }
		public string custom_field_1 {get;set;}
	}
}