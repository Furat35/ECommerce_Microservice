using Dapper;
using Discount.API.Entities;
using Discount.API.Models.Dtos.Coupons;
using FluentValidation;
using Npgsql;
using Shared.Exceptions;

namespace Discount.API.Repositories
{
    // todo: validations should be refactored
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IValidator<Coupon> _couponValidator;

        public DiscountRepository(IConfiguration configuration, IValidator<Coupon> couponValidator)
        {
            _configuration = configuration;
            _couponValidator = couponValidator;
        }

        public async Task<bool> CreateDiscount(CouponAddDto coupon)
        {
            //await ThrowBadRequestIfCouponNotValid(coupon);
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("INSERT INTO COUPON (ProductId, Description, Amount) VALUES (@ProductId, @Description, @Amount)",
                new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2) });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string discountId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE Id=@DiscountId", new { DiscountId = int.Parse(discountId) });

            return affected != 0;
        }

        public async Task<Coupon> GetDiscount(string productId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductId=@ProductId", new { ProductId = productId });

            return coupon == null
                ? new Coupon { ProductId = "No Discount", Amount = 0, Description = "No Discount Desc" }
                : coupon;
        }

        public async Task<bool> UpdateDiscount(CouponUpdateDto coupon)
        {
            //await ThrowBadRequestIfCouponNotValid(coupon);
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE COUPON SET Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2), Id = coupon.Id });

            return affected != 0;
        }

        private async Task ThrowBadRequestIfCouponNotValid(Coupon coupon)
        {
            var validationResult = await _couponValidator.ValidateAsync(coupon);
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
        }
    }
}
