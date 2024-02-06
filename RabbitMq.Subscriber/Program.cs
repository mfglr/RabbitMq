using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://splvvplc:cIISajOBUOptA35p1cIPCfcZ6TIrwIGW@rattlesnake.rmq.cloudamqp.com/splvvplc");

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.BasicQos(0, 1, false);

string queueName = "first_queue";

//If you are sure that the publisher created the queue, the following line can be deleted.
channel.QueueDeclare( queueName, true, false, false );

var subscriber = new EventingBasicConsumer(channel);

channel.BasicConsume(queueName, false, subscriber);



subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
{
	var message = Encoding.UTF8.GetString(e.Body.ToArray());
	Console.WriteLine(message);
	channel.BasicAck(e.DeliveryTag, false);
	Thread.Sleep(1000);
};


Console.ReadLine();


