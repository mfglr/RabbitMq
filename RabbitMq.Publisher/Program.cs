using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://splvvplc:cIISajOBUOptA35p1cIPCfcZ6TIrwIGW@rattlesnake.rmq.cloudamqp.com/splvvplc");

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

string exchangeName = "logs-direct";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Direct);

//bind queues
foreach (var item in Enum.GetNames(typeof(LogNames)))
{
	string routeName = $"route-{item}";
	channel.QueueDeclare(item, true, false, false);
	channel.QueueBind(item, exchangeName, routeName);
}

for (int i = 0; i < 50; i++)
{
	var routeKey = $"route-{(LogNames)(new Random().Next(1, 3))}";
	var messageBody = Encoding.UTF8.GetBytes($"{routeKey} {i}");
	
	channel.BasicPublish(exchangeName,routeKey,false,null,messageBody);
}

Console.WriteLine("The message sent");
Console.ReadLine();

public enum LogNames
{
	Error = 1,
	Info = 2
}
