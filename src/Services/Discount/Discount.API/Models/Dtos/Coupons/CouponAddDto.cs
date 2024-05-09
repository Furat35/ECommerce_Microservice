namespace Discount.API.Models.Dtos.Coupons
{
    public class CouponAddDto
    {
        public string Description { get; set; }
        public string ProductId { get; set; }
        public float Amount { get; set; }
    }
}
