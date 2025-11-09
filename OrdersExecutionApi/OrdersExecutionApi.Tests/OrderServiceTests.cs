using Moq;
using OrdersExecutionApi.Models;
using OrdersExecutionApi.Services;
using Xunit;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrdersExecutionApi.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderExecutor> _mockOrderExecutor;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderExecutor = new Mock<IOrderExecutor>();
            _mockLogger = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_mockOrderExecutor.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldReturnCachedTrade_WhenOrderAlreadyExecuted()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = 10,
                LimitPrice = 150m,
                OrderDate = new DateTime(2025, 11, 10)
            };

            var expectedTrade = new Trade
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                ExecutedQuantity = 10,
                ExecutedPrice = 150m,
                ExecutionTime = new DateTime(2025, 11, 10).AddMinutes(1)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedTrade);

            // Act
            var firstTrade = await _orderService.ExecuteOrderAsync(order);
            var secondTrade = await _orderService.ExecuteOrderAsync(order);

            // Assert
            Assert.Same(firstTrade, secondTrade);
            _mockOrderExecutor.Verify(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldExecuteOrder_WhenOrderNotExecuted()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = 10,
                LimitPrice = 150m,
                OrderDate = new DateTime(2025, 11, 10)
            };

            var expectedTrade = new Trade
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                ExecutedQuantity = 10,
                ExecutedPrice = 150m,
                ExecutionTime = new DateTime(2025, 11, 10).AddMinutes(1)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedTrade);

            // Act
            var trade = await _orderService.ExecuteOrderAsync(order);

            // Assert
            Assert.Equal(expectedTrade.ExecutedQuantity, trade.ExecutedQuantity);
            Assert.Equal(expectedTrade.ExecutedPrice, trade.ExecutedPrice);
            _mockOrderExecutor.Verify(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldThrowArgumentNullException_WhenOrderIsNull()
        {
            // Arrange
            Order order = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _orderService.ExecuteOrderAsync(order));
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldThrowException_WhenOrderExecutorThrowsException()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = 10,
                LimitPrice = 150m,
                OrderDate = new DateTime(2025, 11, 10)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new InvalidOperationException("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.ExecuteOrderAsync(order));
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldHandleMinimumValues()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = 0.00000001m,
                LimitPrice = 0.00000001m,
                OrderDate = new DateTime(2025, 11, 10)
            };

            var expectedTrade = new Trade
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                ExecutedQuantity = 0.00000001m,
                ExecutedPrice = 0.00000001m,
                ExecutionTime = new DateTime(2025, 11, 10).AddMinutes(1)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedTrade);

            // Act
            var trade = await _orderService.ExecuteOrderAsync(order);

            // Assert
            Assert.Equal(expectedTrade.ExecutedQuantity, trade.ExecutedQuantity);
            Assert.Equal(expectedTrade.ExecutedPrice, trade.ExecutedPrice);
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldHandleMaximumValues()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = decimal.MaxValue,
                LimitPrice = decimal.MaxValue,
                OrderDate = new DateTime(2025, 11, 10)
            };

            var expectedTrade = new Trade
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                ExecutedQuantity = decimal.MaxValue,
                ExecutedPrice = decimal.MaxValue,
                ExecutionTime = new DateTime(2025, 11, 10).AddMinutes(1)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedTrade);

            // Act
            var trade = await _orderService.ExecuteOrderAsync(order);

            // Assert
            Assert.Equal(expectedTrade.ExecutedQuantity, trade.ExecutedQuantity);
            Assert.Equal(expectedTrade.ExecutedPrice, trade.ExecutedPrice);
        }

        [Fact]
        public async Task ExecuteOrderAsync_ShouldCompleteWithinReasonableTime()
        {
            // Arrange
            var order = new Order
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                Quantity = 10,
                LimitPrice = 150m,
                OrderDate = new DateTime(2025, 11, 10)
            };

            var expectedTrade = new Trade
            {
                Way = Way.Buy,
                Instrument = "AAPL",
                ExecutedQuantity = 10,
                ExecutedPrice = 150m,
                ExecutionTime = new DateTime(2025, 11, 10).AddMinutes(1)
            };

            _mockOrderExecutor.Setup(x => x.ExecuteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedTrade);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var trade = await _orderService.ExecuteOrderAsync(order);
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Execution took too long");
            Assert.Equal(expectedTrade.ExecutedQuantity, trade.ExecutedQuantity);
            Assert.Equal(expectedTrade.ExecutedPrice, trade.ExecutedPrice);
        }
    }
}