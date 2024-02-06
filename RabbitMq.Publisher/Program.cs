using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://splvvplc:cIISajOBUOptA35p1cIPCfcZ6TIrwIGW@rattlesnake.rmq.cloudamqp.com/splvvplc");

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

string exchangeName = "logs-fanout";
channel.ExchangeDeclare(exchangeName, durable: true, type: ExchangeType.Fanout);


for(int i = 0; i < 50; i++)
{
	var messageBody = Encoding.UTF8.GetBytes($"log {i}");
	channel.BasicPublish(exchangeName,"",null,messageBody);
}


Console.WriteLine("The message sent");
Console.ReadLine();

