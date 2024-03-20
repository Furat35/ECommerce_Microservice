using FluentValidation;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
           
        }
    }
}
