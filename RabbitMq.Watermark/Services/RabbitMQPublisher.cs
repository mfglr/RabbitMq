using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Watermark.Services
{
	public class RabbitMQPublisher : IDisposable
	{
		private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private static string ExchangeName = "WaterMarkExchange";
        private static string RouteKey = "WaterMarkRouteKey";
        private static string QueueName = "WaterMarkQueue";


        public RabbitMQPublisher(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
        }

        public void Connect()
        {
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);
			_channel.QueueDeclare(QueueName, true, false, false, null);
			_channel.QueueBind(QueueName, ExchangeName, RouteKey, null);
		}

        public void PublishMessage(ReadOnlyMemory<byte> data)
        {
            _channel.BasicPublish(ExchangeName,RouteKey,null,data);
        }

        public void PublishEvent(string eventName)
		{
            var body = Encoding.UTF8.GetBytes(eventName);
			_channel.BasicPublish(ExchangeName, RouteKey, null, body);
		}

        public void PublishEvent(object data) 
        {
            var bodyString = JsonConvert.SerializeObject(data);
            var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			_channel.BasicPublish(ExchangeName, RouteKey, null, bodyBytes);
		}

		public void Dispose()
		{
            _connection.Close();
            _connection.Dispose();

            _channel.Close();
            _channel.Dispose();
            
		}
	}
}
