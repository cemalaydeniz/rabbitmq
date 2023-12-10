namespace rabbitmq_server
{
    public class Order
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }

        public override string ToString() => $"Order Id: {Id}\nUserId: {UserId}\nProductId: {ProductId}\nQuantity: {Quantity}";
    }
}
