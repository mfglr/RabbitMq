using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);

string queueName = "deneme";
string routeKey = "deneme";

var subscriber = new EventingBasicConsumer(channel);

channel.BasicConsume(queueName, false, subscriber);

Console.WriteLine("listening erors ...");

subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
{
	var message = Encoding.UTF8.GetString(e.Body.ToArray());
	Console.WriteLine(message);
	channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();


