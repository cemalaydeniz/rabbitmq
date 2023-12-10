using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using rabbitmq_server;
using System.Text;
using System.Text.Json;

//~ Begin - Connection to RabbitMQ
ConnectionFactory factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();
//~ End

channel.ExchangeDeclare("Product", ExchangeType.Topic, true);

/**
 * Give a static queue name in order to read the messages before this consumer is created
 * In this case the name is 'ProductQueue'. Also remove the Bind method
 */
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queueName, "Product", "product.*");
channel.BasicQos(0, 1, false);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queueName, false, consumer);
consumer.Received += (sender, e) =>
{
    if (e.RoutingKey == "product.sold")
    {
        string json = Encoding.UTF8.GetString(e.Body.ToArray());
        Order? order = JsonSerializer.Deserialize<Order>(json);

        Console.WriteLine(order?.ToString());
    }
    else if (e.RoutingKey == "product.refund")
    {
        string orderId = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine($"{orderId} was refunded");
    }
    else return;

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();
