
namespace library_api.DTOs
{
	public class AuthenticationResponse
	{
		public string Token { get; set; }
		public DateTime Expires { get; set; }
	}
}