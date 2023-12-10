using RabbitMQ.Client;
using rabbitmq_server;
using System.Text;
using System.Text.Json;

//~ Begin - Connection to RabbitMQ
ConnectionFactory factory = new ConnectionFactory()
{
    //Uri = new Uri("server url")
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

IConnection connection = factory.CreateConnection();
//~ End


//~ Begin - Messaging methods
void OnSoldProduct(Order order)
{
    using (IModel channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("Product", ExchangeType.Topic, true);

        /**
         * Uncomment it in order to receive the messages that are sent before any consumer is created
         */
        //channel.QueueDeclare("ProductQueue", true, false, false);
        //channel.QueueBind("ProductQueue", "Product", "product.sold");

        string json = JsonSerializer.Serialize(order);
        byte[] package = Encoding.UTF8.GetBytes(json);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish("Product", "product.sold", properties, package);

        Console.WriteLine("The sold product message has been sent");
    }
}

void OnRefundedProduct(string orderId)
{
    using (IModel channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("Product", ExchangeType.Topic, true);

        /**
         * Uncomment it in order to receive the messages that are sent before any consumer is created
         */
        //channel.QueueDeclare("ProductQueue", true, false, false);
        //channel.QueueBind("ProductQueue", "Product", "product.refund");

        byte[] package = Encoding.UTF8.GetBytes(orderId);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish("Product", "product.refund", properties, package);

        Console.WriteLine("The refunded product message has been sent");
    }
}
//~ End


// ... send the messages
OnSoldProduct(new Order()
{
    Id = Guid.NewGuid().ToString(),
    UserId = Guid.NewGuid().ToString(),
    ProductId = Guid.NewGuid().ToString(),
    Quantity = new Random().Next(1, 11)
});

OnRefundedProduct(Guid.NewGuid().ToString());

Console.ReadKey();
