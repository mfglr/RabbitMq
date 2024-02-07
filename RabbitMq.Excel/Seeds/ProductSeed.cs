using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMq.Excel.Models;

namespace RabbitMq.Excel.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new { Id = 1, Name = "Kitap", Description = "Kitap kitap", Stock = 45 },
                new { Id = 2, Name = "Kitap", Description = "Kitap kitap", Stock = 45 },
                  new
                  {
                      Id = 3,
                      Name = "Kitap",
                      Description = "Kitap kitap",
                      Stock = 45
                  },
                   new
                   {
                       Id = 4,
                       Name = "Kitap",
                       Description = "Kitap kitap",
                       Stock = 45
                   },
                    new
                    {
                        Id = 5,
                        Name = "Kitap",
                        Description = "Kitap kitap",
                        Stock = 45
                    },
                     new
                     {
                         Id = 6,
                         Name = "Kitap",
                         Description = "Kitap kitap",
                         Stock = 45
                     }

            );


        }
    }
}
