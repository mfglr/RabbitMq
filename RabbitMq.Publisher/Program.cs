using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var exchangeName = "exchange";

channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true, false, null);

for(int i = 0; i < 100; i++)
{
    var messageBody = Encoding.UTF8.GetBytes($"message-{i}");
    channel.BasicPublish(exchangeName, string.Empty, false, null, messageBody);
    Thread.Sleep(1000);
}

Console.WriteLine("The messages sent");
Console.ReadLine();

