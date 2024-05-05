﻿using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/author")]
	public class AuthorContoller : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;

		public AuthorContoller(ApplicationDBContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<Author>>> Get()
		{
			return await this._context.Author.ToListAsync();
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Author>> Get(int id)
		{
			Author author = await this._context.Author.FirstOrDefaultAsync(au => au.Id == id);
			if (author == null)
			{
				return NotFound();
			}
			return author;
		}

		[HttpGet("{name}")]
		public async Task<ActionResult<Author>> Get(string name)
		{
			Author author = await this._context.Author.FirstOrDefaultAsync(au => au.Name.Contains(name));
			if (author == null)
			{
				return NotFound();
			}
			return author;
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
			return Ok();
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult> Put(Author author, int id)
		{
			if (author.Id != id)
			{
				return BadRequest("Author id does not match with any record.");
			}

			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == id);
			if (!authorExists)
			{
				return NotFound();
			}

			this._context.Update(author);
			await this._context.SaveChangesAsync();
			return Ok();
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
			return Ok();
		}
	}
}