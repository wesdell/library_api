using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using library_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers.V1
{
	[ApiController]
	[Route("api/v1/author")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
	public class AuthorController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;

		public AuthorController(ApplicationDBContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		[HttpGet(Name = "GetAuthorsV1")]
		[AllowAnonymous]
		[ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
		public async Task<ActionResult<List<AuthorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
		{
			IQueryable<Author> queryable = this._context.Author.AsQueryable();
			await HttpContext.InsertPaginationParametersInHeader(queryable);
			List<Author> authors = await queryable.OrderBy(author => author.Name).Paginate(paginationDTO).ToListAsync();
			List<AuthorDTO> authorsDTO = this._mapper.Map<List<AuthorDTO>>(authors);
			return authorsDTO;
		}

		[HttpGet("{id:int}", Name = "GetAuthorByIdV1")]
		[AllowAnonymous]
		[ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
		public async Task<ActionResult<AuthorDTOBooks>> GetById(int id)
		{
			Author author = await this._context.Author.Include(author => author.AuthorBooks).ThenInclude(authorbook => authorbook.Book).FirstOrDefaultAsync(au => au.Id == id);
			if (author == null)
			{
				return NotFound();
			}

			AuthorDTOBooks authorDTOBooks = this._mapper.Map<AuthorDTOBooks>(author);
			return authorDTOBooks;
		}

		[HttpGet("{name}", Name = "GetAuthorByNameV1")]
		[AllowAnonymous]
		public async Task<ActionResult<List<AuthorDTO>>> GetByName([FromRoute] string name)
		{
			List<Author> authors = await this._context.Author.Where(au => au.Name.Contains(name)).ToListAsync();
			return this._mapper.Map<List<AuthorDTO>>(authors);
		}

		[HttpPost(Name = "CreateAuthorV1")]
		public async Task<ActionResult> Post([FromBody] CreateAuthorDTO createAuthorDTO)
		{
			bool authorAlreadyExists = await this._context.Author.AnyAsync(author => author.Name == createAuthorDTO.Name);
			if (authorAlreadyExists)
			{
				return BadRequest($"{createAuthorDTO.Name} already exists.");
			}

			Author author = this._mapper.Map<Author>(createAuthorDTO);
			this._context.Add(author);
			await this._context.SaveChangesAsync();

			AuthorDTO authorDTO = this._mapper.Map<AuthorDTO>(author);

			return CreatedAtRoute("GetAuthorByIdV1", new { id = author.Id }, authorDTO);
		}

		[HttpPut("{id:int}", Name = "UpdateAuthorByIdV1")]
		public async Task<ActionResult> Put(CreateAuthorDTO newAuthor, int id)
		{
			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == id);
			if (!authorExists)
			{
				return NotFound();
			}

			Author author = this._mapper.Map<Author>(newAuthor);
			author.Id = id;

			this._context.Update(author);
			await this._context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "DeleteAuthorByIdV1")]
		public async Task<ActionResult> Delete(int id)
		{
			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == id);
			if (!authorExists)
			{
				return NotFound();
			}

			this._context.Remove(new Author() { Id = id });
			await this._context.SaveChangesAsync();
			return NoContent();
		}
	}
}