using library_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace library_api
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions options) : base(options) { }

		public DbSet<Author> Author { get; set; }
		public DbSet<Book> Book { get; set; }
	}
}