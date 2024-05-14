using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/book/{bookId:int}/comment")]
	public class CommentController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;

		public CommentController(ApplicationDBContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		[HttpGet("{id:int}", Name = "GetCommentById")]
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

		[HttpGet]
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

		[HttpPost]
		public async Task<ActionResult> Post(int bookId, [FromBody] CreateCommentDTO createCommentDTO)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == bookId);
			if (!bookExists)
			{
				return NotFound();
			}
			Comment comment = this._mapper.Map<Comment>(createCommentDTO);
			comment.BookId = bookId;
			this._context.Add(comment);
			await this._context.SaveChangesAsync();

			CommentDTO commentDTO = this._mapper.Map<CommentDTO>(comment);

			return CreatedAtRoute("GetCommentById", new { id = comment.Id, bookId }, commentDTO);
		}
	}
}