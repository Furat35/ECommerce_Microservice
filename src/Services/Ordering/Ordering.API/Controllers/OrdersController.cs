using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Filters;
using Ordering.Application.Helpers;
using Ordering.Application.Models.Dtos.Orders;
using Shared.Constants;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet(Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders([FromQuery] OrderRequestFilter filters)
        {
            var query = new GetOrdersListQuery(HttpContext.User.GetActiveUserId(), filters);
            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var query = new GetOrderByIdQuery(id);
            var order = await _mediator.Send(query);

            return Ok(order);
        }

        [HttpPut(Name = "UpdateOrder")]
        [Authorize(Roles = $"{Role.User}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [Authorize(Roles = $"{Role.User}")]
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
