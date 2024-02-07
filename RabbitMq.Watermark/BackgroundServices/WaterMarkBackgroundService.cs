using Newtonsoft.Json;
using RabbitMq.Watermark.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Text;

namespace RabbitMq.Watermark.BackgroundServices
{
	public class WaterMarkBackgroundService : BackgroundService
	{

		private readonly ConnectionFactory _connectionFactory;
		private IConnection _connection;
		private IModel _channel;

		private static string ExchangeName = "WaterMarkExchange";
		private static string RouteKey = "WaterMarkRouteKey";
		private static string QueueName = "WaterMarkQueue";


		public WaterMarkBackgroundService(ConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_connection = _connectionFactory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.BasicQos(0, 1, false);

			return base.StartAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{

			var subscriber = new AsyncEventingBasicConsumer(_channel);
			_channel.BasicConsume(QueueName, false, subscriber);

			subscriber.Received += Subscriber_Received;

			return Task.CompletedTask;
		}

		private Task Subscriber_Received(object sender, BasicDeliverEventArgs @event)
		{

			try
			{
				var productImageCreatedEvent = JsonConvert.DeserializeObject<ProductImageCreatedEvent>(
					Encoding.UTF8.GetString(@event.Body.ToArray())
				);

				var imageName = productImageCreatedEvent.ImageName;

				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

				using var img = Image.FromFile(path);
				using var graphic = Graphics.FromImage(img);
				var font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold, GraphicsUnit.Pixel);
				string text = "www.takasEt.com";
				var color = Color.FromArgb(128, 255, 255, 255);
				var brush = new SolidBrush(color);

				var textSize = graphic.MeasureString(text, font);

				int paddingLeft = 25;
				int paddingBottom = 25;

				float x = img.Width - textSize.Width - paddingLeft;
				float y = img.Height - textSize.Height - paddingBottom;

				graphic.DrawString(text, font, brush, new PointF(x, y));

				img.Save("wwwroot/images/watermarks/" + imageName);

				_channel.BasicAck(@event.DeliveryTag, false);
				
				img.Dispose();
				graphic.Dispose();

				Console.WriteLine("deneme");
			}
			catch (Exception ex)
			{
				Console.WriteLine("hata");
			}

			return Task.CompletedTask;



		}
	}
}
