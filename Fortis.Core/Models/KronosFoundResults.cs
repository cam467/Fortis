namespace KnowBe4.Core.Models
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;

    public class KronosFoundResults
	{
		public List<User> success {get;set;}
		public List<User> failed { get; set; }
	}
}