using library_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace library_api
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AuthorBook>().HasKey(el => new { el.AuthorId, el.BookId });
		}

		public DbSet<Author> Author { get; set; }
		public DbSet<Book> Book { get; set; }
		public DbSet<Comment> Comment { get; set; }
		public DbSet<AuthorBook> AuthorBook { get; set; }
	}
}