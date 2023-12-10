using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using rabbitmq_server;
using System.Text;
using System.Text.Json;

//~ Begin - Connection to RabbitMQ
ConnectionFactory factory = new ConnectionFactory()
{
    //Uri = new Uri("server url"),
    HostName = "localhost",
    Port = 5672
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();
//~ End

channel.ExchangeDeclare("Report", ExchangeType.Direct, true);

/**
 * Give a static queue name in order to read the messages before this consumer is created
 * In this case the name is 'ReportQueue'. Also remove the Bind method
 */
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queueName, "Report", "admin");
channel.BasicQos(0, 1, false);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queueName, false, consumer);
consumer.Received += (sender, e) =>
{
    string json = Encoding.UTF8.GetString(e.Body.ToArray());
    Report? report = JsonSerializer.Deserialize<Report>(json);

    Console.WriteLine(report?.ToString());

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();
