namespace Fortis.Core.Models
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public class KronosFoundResults
	{
		public List<User> success {get;set;}
		public List<User> failed { get; set; }
	}
}