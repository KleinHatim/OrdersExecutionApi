using Microsoft.AspNetCore.Mvc;
using OrdersExecutionApi.Models;
using OrdersExecutionApi.Services;

namespace OrdersExecutionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteOrder([FromBody] Order order, CancellationToken cancellationToken = default)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null");
            }

            try
            {
                var trade = await _orderService.ExecuteOrderAsync(order, cancellationToken);
                return Ok(trade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}