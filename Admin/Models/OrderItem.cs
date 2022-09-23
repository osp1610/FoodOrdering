namespace LoginPage.Models
{
    public class OrderItem
    {
        public const string Order_put = "ORDER_PROCESSING";
        public const string Order_del = "ORDER_DELIVERD";
        public const string Order_cancelled = "ORDER_CANCELLED";

        public OrderItem()
        {
        }

        public OrderItem(int id, string name, int qty, int cost, string userName, string address, string orderStatus)
        {
            Id = id;
            Name = name;
            Qty = qty;
            Cost = cost;
            UserName = userName;
            Address = address;
            OrderStatus = orderStatus;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Cost { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string OrderStatus { get; set; }

        public override string? ToString()
        {
            return Name+"-"+OrderStatus;
        }

        //TODO: Add attribute description



    }
}
