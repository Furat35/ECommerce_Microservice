using AutoMapper;
using EventBus.Message.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {result}");
        }

        //private bool CheckIfUserIsAuthenticated(Headers headers)
        //{
        //    if (!headers.TryGetHeader("Authorization", out var tokenObj) || !(tokenObj is string token))
        //    {
        //        _logger.LogError("Authorization token is missing from the message.");
        //        return false;
        //    }
        //    token = token.Replace("Bearer ", "");
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = _configuration["JWTAuth:ValidIssuerURL"],
        //        ValidAudience = _configuration["JWTAuth:ValidAudienceURL"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:SecretKey"])),
        //    };
        //    var validationResult = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        //    return validationResult.Identities.Any(_ => _.IsAuthenticated);
        //}
    }
}
