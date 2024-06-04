using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers.V1
{
	[ApiController]
	[Route("api/v1/book")]
	public class BookController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;

		public BookController(ApplicationDBContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		[HttpGet("{id:int}", Name = "GetBookByIdV1")]
		public async Task<ActionResult<BookDTOAuthors>> Get(int id)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == id);
			if (!bookExists)
			{
				return NotFound();
			}
			Book book = await this._context.Book.Include(book => book.AuthorBooks).ThenInclude(authorbook => authorbook.Author).FirstOrDefaultAsync(book => book.Id == id);
			book.AuthorBooks = book.AuthorBooks.OrderBy(book => book.Order).ToList();
			return this._mapper.Map<BookDTOAuthors>(book);
		}

		[HttpPost(Name = "CreateBookV1")]
		public async Task<ActionResult> Post([FromBody] CreateBookDTO createBookDTO)
		{
			if (createBookDTO.AuthorsIds == null)
			{
				return BadRequest("Cannot create a book without at least one author.");
			}

			List<int> authorsIds = await this._context.Author.Where(author => createBookDTO.AuthorsIds.Contains(author.Id)).Select(author => author.Id).ToListAsync();
			if (createBookDTO.AuthorsIds.Count != authorsIds.Count)
			{
				return BadRequest("There is no one of the submitted authors.");
			}

			Book book = this._mapper.Map<Book>(createBookDTO);
			this.SetAuthorsOrder(book);
			this._context.Add(book);
			await this._context.SaveChangesAsync();

			BookDTO bookDTO = this._mapper.Map<BookDTO>(book);
			return CreatedAtRoute("GetBookById", new { id = book.Id }, bookDTO);
		}

		[HttpPut("{id:int}", Name = "UpdateBookByIdV1")]
		public async Task<ActionResult> Put(CreateBookDTO newBook, int id)
		{
			Book bookDB = await this._context.Book.Include(book => book.AuthorBooks).FirstOrDefaultAsync(book => book.Id == id);
			if (bookDB == null)
			{
				return NotFound();
			}

			bookDB = this._mapper.Map(newBook, bookDB);
			this.SetAuthorsOrder(bookDB);
			await this._context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPatch("{id:int}", Name = "PartialUpdateBookByIdV1")]
		public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
		{
			if (patchDocument == null)
			{
				return BadRequest();
			}

			Book bookDB = await this._context.Book.FirstOrDefaultAsync(book => book.Id == id);
			if (bookDB == null)
			{
				return NotFound();
			}

			BookPatchDTO bookPatchDTO = this._mapper.Map<BookPatchDTO>(bookDB);
			patchDocument.ApplyTo(bookPatchDTO, ModelState);

			bool isValidBook = TryValidateModel(bookPatchDTO);
			if (!isValidBook)
			{
				return BadRequest();
			}

			this._mapper.Map(bookPatchDTO, bookDB);
			await this._context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "DeleteBookByIdV1")]
		public async Task<ActionResult> Delete(int id)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == id);
			if (!bookExists)
			{
				return NotFound();
			}

			this._context.Remove(new Book() { Id = id });
			await this._context.SaveChangesAsync();
			return NoContent();
		}

		private void SetAuthorsOrder(Book book)
		{
			if (book.AuthorBooks != null)
			{
				for (int i = 0; i < book.AuthorBooks.Count; i += 1)
				{
					book.AuthorBooks[i].Order = i;
				}
			}
		}
	}
}