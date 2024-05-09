using Discount.API.Entities;
using Discount.API.Models.Dtos.Coupons;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productId);
        Task<bool> CreateDiscount(CouponAddDto coupon);
        Task<bool> UpdateDiscount(CouponUpdateDto coupon);
        Task<bool> DeleteDiscount(string discountId);
    }
}
