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
var messageBody = Encoding.UTF8.GetBytes(messaj);

//default exchange
channel.BasicPublish(string.Empty, queueName, null, messageBody);
Console.WriteLine("The message sent");
Console.ReadLine();

