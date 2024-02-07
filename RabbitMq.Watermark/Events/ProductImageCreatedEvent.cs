namespace RabbitMq.Watermark.Events
{
	public class ProductImageCreatedEvent
	{
		public string ImageName { get; private set; }

		public ProductImageCreatedEvent(string imageName)
		{
			ImageName = imageName;
		}
	}
}
