using OrdersExecutionApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersExecutionApi.Services
{
    public interface IOrderService
    {
        Task<Trade> ExecuteOrderAsync(Order order, CancellationToken cancellationToken = default);
    }
}