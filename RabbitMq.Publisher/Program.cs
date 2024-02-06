using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://splvvplc:cIISajOBUOptA35p1cIPCfcZ6TIrwIGW@rattlesnake.rmq.cloudamqp.com/splvvplc");

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

string exchangeName = "logs-topic";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Topic);

Random random = new Random();

for (int i = 0; i < 50; i++)
{
	LogNames log1 = (LogNames)random.Next(1, 3);
	LogNames log2 = (LogNames)random.Next(1, 3);
	
	string routeKey = $"{log1}.{log2}";
	Console.WriteLine(routeKey);
	var messageBody = Encoding.UTF8.GetBytes($"{routeKey} {i}");
	
	channel.BasicPublish(exchangeName,routeKey,false,null,messageBody);
}

Console.WriteLine("The logs sent");
Console.ReadLine();

public enum LogNames
{
	Error = 1,
	Info = 2
}
