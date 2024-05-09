using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace ECommerce.UI.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> Index([FromQuery] OrderRequestFilter filters)
        {
            var orders = await _orderService.GetOrders(filters);
            return View(orders);
        }

        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> GetOrder([FromQuery] string orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return View(order);
        }
    }
}
