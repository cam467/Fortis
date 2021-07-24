namespace KnowBe4.Core.Entities
{
    using System;
    using System.Collections.Generic;

    public class Campaign
	{
		public long campaign_id { get; set; }
		public string name { get; set; }
		public List<KGroup> groups { get; set; }
		public string status { get; set; }
		public List<StorePurchase> modules { get; set; }
		public List<StorePurchase> content { get; set; }
		public string duration_type { get; set; }
		public DateTime start_date { get; set; }
		public DateTime end_date { get; set; }
		public string relative_duration { get; set; }
		public bool auto_enroll { get; set; }
		public bool allow_multiple_enrollments { get; set; }
	}
}