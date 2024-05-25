using library_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class CreateCommentDTO
	{
		[Required]
		public string Content { get; set; }
	}
}