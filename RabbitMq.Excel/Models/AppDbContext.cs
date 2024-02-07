using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace RabbitMq.Excel.Models
{
	public class AppDbContext : IdentityDbContext
	{
		public DbSet<UserFile> UserFiles { get; set; }
		public DbSet<Product> Products { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}
