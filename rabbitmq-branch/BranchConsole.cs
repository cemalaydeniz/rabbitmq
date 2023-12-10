using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//~ Begin - Connection to RabbitMQ
ConnectionFactory factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();
//~ End

channel.ExchangeDeclare("Announcement", ExchangeType.Fanout, true);

/**
 * Give a static queue name in order to read the messages before this consumer is created
 * In this case the name is 'AnnouncementQueue'. Also remove the Bind method
 */
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queueName, "Announcement", string.Empty);
channel.BasicQos(0, 1, false);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queueName, false, consumer);
consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();
