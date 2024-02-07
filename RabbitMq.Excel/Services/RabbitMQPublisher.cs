using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Excel.Services
{
	public class RabbitMQPublisher : IDisposable
	{
		private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        private static string ExchangeName = "ExcekExchange";
        private static string RouteKey = "ExcelRouteKey";
        private static string QueueName = "ExcelQueue";


        public RabbitMQPublisher(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            
        }

        public void Connect()
        {

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.CreateBasicProperties().Persistent = true;


            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);
            _channel.QueueDeclare(QueueName, true, false, false, null);
			_channel.QueueBind(QueueName, ExchangeName, RouteKey, null);
        }

        public bool ConnectionIsOpen()
        {
            return _connection != null && _connection.IsOpen;
        }

        public void Publish(ReadOnlyMemory<byte> data)
        {
            _channel.BasicPublish(ExchangeName,RouteKey,null,data);
        }

        public void Publish(string eventName)
		{
            var body = Encoding.UTF8.GetBytes(eventName);
			_channel.BasicPublish(ExchangeName, RouteKey, null, body);
		}

        public void Publish(object data) 
        {
            var bodyString = JsonConvert.SerializeObject(data);
            var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
			_channel.BasicPublish(ExchangeName, RouteKey, null, bodyBytes);
		}

		public void Dispose()
		{
            _connection?.Close();
            _connection?.Dispose();

            _channel?.Close();
            _channel?.Dispose();
            
		}
	}
}
