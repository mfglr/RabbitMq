using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMq.Watermark.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		public int Stock {  get; set; }
		public string PictureUrl { get; set; }
	}
}
