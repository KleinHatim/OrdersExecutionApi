using OrdersExecutionApi.Models;

namespace OrdersExecutionApi.Services
{
    public class OrderExecutor : IOrderExecutor
    {
        private static readonly Random RandomInstance = new Random();

        public async Task<Trade> ExecuteOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Simulation d'un délai d'exécution
            await Task.Delay(RandomInstance.Next(200, 1000), cancellationToken);

            return new Trade
            {
                Way = order.Way,
                Instrument = order.Instrument,
                ExecutedQuantity = order.Quantity,
                ExecutedPrice = order.LimitPrice,
                ExecutionTime = order.OrderDate.AddMinutes(1)
            };
        }
    }
}