namespace LoginPage.Models
{
    public class OrderItem
    {
        public const string Order_put = "ORDER_PROCESSING";
        public const string Order_del = "ORDER_DELIVERD";
        public const string Order_cancelled = "ORDER_CANCELLED";
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Cost { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string OrderStatus { get; set; }

        //TODO: Add attribute description

    }
}
