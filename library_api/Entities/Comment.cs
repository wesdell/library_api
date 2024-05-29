using library_api.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace library_api.Entities
{
	public class Comment : IComment
	{
		public int Id { get; set; }
		[Required]
		public string Content { get; set; }
		public int BookId { get; set; }
		public Book Book { get; set; }
		public string UserId { get; set; }
		public IdentityUser User { get; set; }
	}
}