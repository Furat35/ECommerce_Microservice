using AutoMapper;
using EventBus.Message.Events;
using FluentValidation;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using SendGrid.Helpers.Errors.Model;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IValidator<BasketCheckoutEvent> validator) : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger = logger;
        private readonly IValidator<BasketCheckoutEvent> _validator = validator;

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var validationResult = await _validator.ValidateAsync(context.Message);
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {result}");
        }
    }
}
