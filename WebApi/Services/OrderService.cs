using System;
using System.Threading.Tasks;
using WebApi.DTO.Request;
using WebApi.DTO.Response;
using WebApi.Models;
using WebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class OrderService
    {
        private readonly WebApiDBContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(WebApiDBContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<object> CreateOrder(CreateOrderRequestDTO orderRequest)
        {
            try
            {
                var order = new Order
                {
                    UserId = orderRequest.UserId,
                    ProductId = orderRequest.ProductId,
                    TotalItem = orderRequest.TotalItem,
                    TotalOrderPrice = orderRequest.TotalOrderPrice,
                    CreatedTime = DateTime.Now

                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var response = new
                {
                    orderId = order.OrderId,
                    message = "Order created successfully",
                    statusCode = StatusCodes.Status201Created,
                    status = "Created"
                };

                _logger.LogInformation($"Order created successfully for user {orderRequest.UserId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create order");
                return new
                {
                    message = "Failed to create order. Please try again.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error"
                };
            }
        }

    }
}
