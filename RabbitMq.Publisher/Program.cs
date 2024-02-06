using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://splvvplc:cIISajOBUOptA35p1cIPCfcZ6TIrwIGW@rattlesnake.rmq.cloudamqp.com/splvvplc");

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

string queueName = "first_queue";

channel.QueueDeclare(
	queueName,
	true,
	false,
	false
);

string messaj = "first message";

//default exchange

for(int i = 0; i < 50; i++)
{

	var messageBody = Encoding.UTF8.GetBytes($"{messaj} {i}");
	channel.BasicPublish(string.Empty, queueName, null, messageBody);
}


Console.WriteLine("The message sent");
Console.ReadLine();

