using Dapper;
using Discount.API.Entities;
using Microsoft.AspNetCore.Mvc;
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
            var affected = await connection.ExecuteAsync("INSERT INTO COUPON (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2) });
            var x = new global::Discount.API.Repositories.DiscountRepository(_configuration);
            return affected == 0
                ? false
                : true;
        }

        [HttpDelete("{productName}")]
        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE UPPER(ProductName)=@ProductName", new { ProductName = productName.ToUpper() });
            return affected == 0
                ? false
                : true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE UPPER(ProductName)=@ProductName", new { ProductName = productName.ToUpper() });
            return coupon == null
                ? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" }
                : coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE COUPON SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = Math.Round(coupon.Amount, 2), Id = coupon.Id });
            return affected == 0
                ? false
                : true;
        }
    }
}
