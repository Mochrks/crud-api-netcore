using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/orders/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO orderRequest)
        {
            try
            {
                var response = await _orderService.CreateOrder(orderRequest);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
