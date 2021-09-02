namespace Fortis.Core.Models
{
	public class Login
	{
		public string utf8 { get; set; }
		public string authenticity_token { get; set; }
		public LoginUser user { get; set; }
		public string commit { get; set; }
	}
}