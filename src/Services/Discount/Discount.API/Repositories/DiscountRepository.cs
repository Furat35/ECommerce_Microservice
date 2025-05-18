using Dapper;
using Discount.API.Entities;
using Discount.API.Models.Dtos.Coupons;
using Npgsql;
using Shared.Helpers.interfaces;

namespace Discount.API.Repositories
{
    // todo: validations should be refactored
    public class DiscountRepository(IConfiguration configuration, ICustomFluentValidationErrorHandling customValidator) : IDiscountRepository
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ICustomFluentValidationErrorHandling _customValidator = customValidator;

        public async Task<bool> CreateDiscount(CouponAddDto coupon)
        {
            await _customValidator.ValidateAndThrowAsync(coupon);
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

            return coupon ?? new Coupon { ProductId = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }

        public async Task<bool> UpdateDiscount(CouponUpdateDto coupon)
        {
            await _customValidator.ValidateAndThrowAsync(coupon);
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE COUPON SET Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2), Id = coupon.Id });

            return affected != 0;
        }
    }
}
