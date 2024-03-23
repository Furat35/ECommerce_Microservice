using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Helpers;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetOrdersListQuery(HttpContext.User.GetActiveUserId());
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        //for testing
        //[HttpPost(Name = "CheckoutOrder")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        //{
        //    var orderId = await _mediator.Send(command);
        //    return Ok(new { OrderId = orderId });
        //}

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var command = new DeleteOrderCommand { Id = Guid.Parse(id) };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
