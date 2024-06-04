using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace library_api.Controllers.V1
{
	[ApiController]
	[Route("api/v1/book/{bookId:int}/comment")]
	public class CommentController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;

		public CommentController(ApplicationDBContext context, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			this._context = context;
			this._mapper = mapper;
			this._userManager = userManager;
		}

		[HttpGet("{id:int}", Name = "GetCommentByIdV1")]
		public async Task<ActionResult<CommentDTO>> GetById(int id)
		{
			bool commentExists = await this._context.Comment.AnyAsync(comment => comment.Id == id);
			if (!commentExists)
			{
				return NotFound();
			}
			Comment comment = await this._context.Comment.FirstOrDefaultAsync(comment => comment.Id == id);
			return this._mapper.Map<CommentDTO>(comment);
		}

		[HttpGet(Name = "GetCommentsByBookIdV1")]
		public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == bookId);
			if (!bookExists)
			{
				return NotFound();
			}
			List<Comment> comments = await this._context.Comment.Where(comment => comment.BookId == bookId).ToListAsync();
			return this._mapper.Map<List<CommentDTO>>(comments);
		}

		[HttpPost(Name = "CreateCommentByBookIdV1")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult> Post(int bookId, [FromBody] CreateCommentDTO createCommentDTO)
		{
			Claim emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Email).FirstOrDefault();
			string email = emailClaim.Value;
			IdentityUser user = await this._userManager.FindByEmailAsync(email);
			string userId = user.Id;

			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == bookId);
			if (!bookExists)
			{
				return NotFound();
			}

			Comment comment = this._mapper.Map<Comment>(createCommentDTO);
			comment.BookId = bookId;
			comment.UserId = userId;
			this._context.Add(comment);
			await this._context.SaveChangesAsync();

			CommentDTO commentDTO = this._mapper.Map<CommentDTO>(comment);

			return CreatedAtRoute("GetCommentById", new { id = comment.Id, bookId }, commentDTO);
		}

		[HttpPut("{id:int}", Name = "UpdateCommentByBookIdV1")]
		public async Task<ActionResult> Put(CreateCommentDTO newComment, int bookId, int id)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == bookId);
			if (!bookExists)
			{
				return NotFound();
			}

			bool commentExists = await this._context.Comment.AnyAsync(comment => comment.Id == id);
			if (!commentExists)
			{
				return NotFound();
			}

			Comment comment = this._mapper.Map<Comment>(newComment);
			comment.Id = id;
			comment.BookId = bookId;
			this._context.Add(comment);
			await this._context.SaveChangesAsync();
			return NoContent();
		}
	}
}