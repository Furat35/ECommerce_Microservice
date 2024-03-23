using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;


        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("INSERT INTO COUPON (ProductId, Description, Amount) VALUES (@ProductId, @Description, @Amount)",
                new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2) });

            return affected == 0
                ? false
                : true;
        }

        public async Task<bool> DeleteDiscount(string discountId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE Id=@DiscountId", new { DiscountId = int.Parse(discountId) });

            return affected == 0
                ? false
                : true;
        }

        public async Task<Coupon> GetDiscount(string discountId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE Id=@DiscountId", new { DiscountId = int.Parse(discountId) });

            return coupon == null
                ? new Coupon { ProductId = "No Discount", Amount = 0, Description = "No Discount Desc" }
                : coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE COUPON SET ProductId=@ProductId, Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2), Id = coupon.Id });

            return affected == 0
                ? false
                : true;
        }
    }
}
