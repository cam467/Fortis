namespace Fortis.Core.Models
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public class ReportStatsView
	{
		public int addedcount { get; set; }
		public int updatecount { get; set; }
		public int deletedcount { get; set; }
		public List<User> addedusers { get; set; }
		public List<User> deletedusers { get; set; }
		public List<User> nokronosusers { get; set; }
		public string urllink { get; set; }
	}
}