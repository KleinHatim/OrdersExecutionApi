namespace OrdersExecutionApi.Models
{
    public class Trade
    {
        public Way Way { get; set; }
        public string Instrument { get; set; }
        public decimal ExecutedQuantity { get; set; }
        public decimal ExecutedPrice { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
