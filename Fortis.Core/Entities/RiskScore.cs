namespace KnowBe4.Core.Entities
{
    using System;
    
    public class RiskScore
	{
		public double risk_score { get; set; }
		public string score_type {get;set;}
		public string score_id {get;set;}
		public DateTime date { get; set; }
	}
}