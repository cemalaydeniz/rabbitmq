namespace rabbitmq_server
{
    public class Report
    {
        public class Item
        {
            public string ProductId { get; set; } = null!;
            public int Quantity { get; set; }
        }

        public List<Item> Items { get; set; } = null!;

        public override string ToString()
        {
            if (Items.Count == 0)
                return "No item";

            string output = "";
            Items.ForEach(_ => output += $"{_.ProductId} was sold {_.Quantity} times\n");

            return output;
        }
    }
}
