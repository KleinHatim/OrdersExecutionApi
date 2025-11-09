namespace OrdersExecutionApi.Models
{
    public enum Way
    {
        Buy = 1,
        Sell = 2
    }

    public class Order
    {
        public Way Way { get; set; }
        public string Instrument { get; set; }
        public decimal Quantity { get; set; }
        public decimal LimitPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
