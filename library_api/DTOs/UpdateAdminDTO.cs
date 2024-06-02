using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class UpdateAdminDTO
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}