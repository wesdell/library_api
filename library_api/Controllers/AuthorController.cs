using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/author")]
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

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<AuthorDTO>>> Get()
		{
			List<Author> authors = await this._context.Author.ToListAsync();
			return this._mapper.Map<List<AuthorDTO>>(authors);
		}

		[HttpGet("{id:int}", Name = "GetAuthorById")]
		public async Task<ActionResult<AuthorDTOBooks>> Get(int id)
		{
			Author author = await this._context.Author.Include(author => author.AuthorBooks).ThenInclude(authorbook => authorbook.Book).FirstOrDefaultAsync(au => au.Id == id);
			if (author == null)
			{
				return NotFound();
			}
			return this._mapper.Map<AuthorDTOBooks>(author);
		}

		[HttpGet("{name}")]
		public async Task<ActionResult<List<AuthorDTO>>> Get([FromRoute] string name)
		{
			List<Author> authors = await this._context.Author.Where(au => au.Name.Contains(name)).ToListAsync();
			return this._mapper.Map<List<AuthorDTO>>(authors);
		}

		[HttpPost]
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

			return CreatedAtRoute("GetAuthorById", new { id = author.Id }, authorDTO);
		}

		[HttpPut("{id:int}")]
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

		[HttpDelete("{id:int}")]
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