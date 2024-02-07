using RabbitMQ.Client;

namespace FileCreaterWorkerService.Services
{
    public class RabbitMQSubscriber : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        private static string QueueName = "ExcelQueue";

        public RabbitMQSubscriber(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);
        }

        public bool ConnectionIsOpen() => _connection != null && _connection.IsOpen;


        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
