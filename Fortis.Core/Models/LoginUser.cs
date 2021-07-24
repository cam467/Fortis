namespace KnowBe4.Core.Models
{
    public class LoginUser
	{
		public string email { get; set; }
		public string password { get; set; }
		public int remember_me { get; set; }
	}
}