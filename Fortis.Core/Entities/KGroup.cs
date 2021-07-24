namespace KnowBe4.Core.Entities
{
    using System;
    using System.Collections.Generic;
    public class KGroup
	{
		public int id { get; set; }
		public int group_id { get {return id;} }
		public string name { get; set; }
		public string group_type { get; set; }
		public string adi_guid { get; set; }
		public int member_count { get; set; }
		public decimal current_risk_score { get; set; }
		public List<RiskScore> risk_score_history { get; set; }
		public string status { get; set; }
	}       
}