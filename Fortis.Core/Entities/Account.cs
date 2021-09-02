namespace Fortis.Core.Entities
{
    using System;
    using System.Collections.Generic;

    public class Account
	{
		public string name { get; set; }
		public string type { get; set; }
		public List<string> domains { get; set; }
		public List<User> admins { get; set; }
		public string subscription_level { get; set; }
		public DateTime subscription_end_date { get; set; }
		public int number_of_seats { get; set; }
		public decimal current_risk_score { get; set; }
		public List<RiskScore> risk_score_history { get; set; }
	}
}