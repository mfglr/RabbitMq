using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("deneme", false, false, false,null);
	
string routeKey = $"deneme";
var messageBody = Encoding.UTF8.GetBytes($"message");

channel.BasicPublish(string.Empty,routeKey,false,null,messageBody);

Console.WriteLine("The logs sent");
Console.ReadLine();

