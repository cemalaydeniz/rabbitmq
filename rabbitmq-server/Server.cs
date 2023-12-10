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

void SendAnnouncement(string message)
{
    using (IModel channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("Announcement", ExchangeType.Fanout, true);

        /**
         * Uncomment it in order to receive the messages that are sent before any consumer is created
         */
        //channel.QueueDeclare("AnnouncementQueue", true, false, false);
        //channel.QueueBind("AnnouncementQueue", "Announcement", string.Empty);

        byte[] package = Encoding.UTF8.GetBytes(message);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish("Announcement", string.Empty, properties, package);

        Console.WriteLine("The announcement has been sent");
    }
}

void SendReport(Report report)
{
    using (IModel channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("Report", ExchangeType.Direct, true);

        /**
         * Uncomment it in order to receive the messages that are sent before any consumer is created
         */
        //channel.QueueDeclare("ReportQueue", true, false, false);
        //channel.QueueBind("ReportQueue", "Report", "admin");

        string json = JsonSerializer.Serialize(report);
        byte[] package = Encoding.UTF8.GetBytes(json);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish("Report", "admin", properties, package);
        // ... There could be more different report types to send

        Console.WriteLine("The report has been sent");
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

SendAnnouncement("20% off for all products");

SendReport(new Report()
{
    Items = new List<Report.Item>()
    {
        new Report.Item() { ProductId = Guid.NewGuid().ToString(), Quantity = new Random().Next(1, 11) },
        new Report.Item() { ProductId = Guid.NewGuid().ToString(), Quantity = new Random().Next(1, 11) },
        new Report.Item() { ProductId = Guid.NewGuid().ToString(), Quantity = new Random().Next(1, 11) },
        new Report.Item() { ProductId = Guid.NewGuid().ToString(), Quantity = new Random().Next(1, 11) },
        new Report.Item() { ProductId = Guid.NewGuid().ToString(), Quantity = new Random().Next(1, 11) }
    }
});

Console.ReadKey();
