using OrdersExecutionApi.Models;

namespace OrdersExecutionApi.Services
{
    public interface IOrderExecutor
    {
        Task<Trade> ExecuteOrderAsync(Order order, CancellationToken cancellationToken = default);
    }
}