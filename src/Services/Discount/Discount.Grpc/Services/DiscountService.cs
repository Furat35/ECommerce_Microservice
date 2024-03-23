using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = $"{Role.Admin}")]
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. DiscountId : {DiscountId}", coupon.Id);
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        [Authorize(Roles = $"{Role.Admin}")]
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.DiscountId);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductId);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for Product Id={request.ProductId} is not found."));
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        [Authorize(Roles = $"{Role.Admin}")]
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. DiscountId : {DiscountId}", coupon.Id);
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }
    }
}
