using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
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
            var affected = await connection.ExecuteAsync("INSERT INTO COUPON (ProductName, Description, Amount) VALUES (@ProductId, @Description, @Amount)",
                new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2) });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string discountId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE Id=@DiscountId", new { DiscountId = discountId });

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

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE COUPON SET ProductId=@ProductId, Description=@Description, Amount=@Amount WHERE Id = @Id", new { ProductId = coupon.ProductId, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2), Id = coupon.Id });

            return affected != 0;
        }
    }
}
