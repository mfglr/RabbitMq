using Microsoft.EntityFrameworkCore;

namespace RabbitMq.Watermark.Models
{
	public class Context : DbContext
	{

        public DbSet<Product> Products { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }




    }
}
