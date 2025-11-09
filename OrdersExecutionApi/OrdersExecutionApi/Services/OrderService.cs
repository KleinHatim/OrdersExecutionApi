using OrdersExecutionApi.Models;
using System.Collections.Concurrent;

namespace OrdersExecutionApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderExecutor _orderExecutor;
        private readonly ILogger<OrderService> _logger;
        private readonly ConcurrentDictionary<string, Trade> _tradeCache;

        public OrderService(IOrderExecutor orderExecutor, ILogger<OrderService> logger)
        {
            _orderExecutor = orderExecutor ?? throw new ArgumentNullException(nameof(orderExecutor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tradeCache = new ConcurrentDictionary<string, Trade>();
        }

        public async Task<Trade> ExecuteOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            var orderKey = GenerateOrderKey(order);

            if (_tradeCache.TryGetValue(orderKey, out var cachedTrade))
            {
                _logger.LogInformation("Returning cached trade for order: {OrderKey}", orderKey);
                return cachedTrade;
            }

            try
            {
                _logger.LogInformation("Executing order: {OrderKey}", orderKey);
                var trade = await _orderExecutor.ExecuteOrderAsync(order, cancellationToken);
                _tradeCache.TryAdd(orderKey, trade);
                return trade;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute order: {OrderKey}", orderKey);
                throw;
            }
        }

        private string GenerateOrderKey(Order order)
        {
            return $"{order.Way}-{order.Instrument}-{order.Quantity}-{order.LimitPrice}-{order.OrderDate:o}";
        }
    }
}