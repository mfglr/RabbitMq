using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);


var exchangeName = "exchange";
string queueName = "deneme1";
string routeKey = "deneme1";

channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, exchangeName, routeKey);

var subscriber = new AsyncEventingBasicConsumer(channel);
channel.BasicConsume(queueName, false, subscriber);

Console.WriteLine("listening erors ...");

subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(message);
    channel.BasicAck(e.DeliveryTag, false);
    return Task.CompletedTask;
};


Console.ReadLine();


