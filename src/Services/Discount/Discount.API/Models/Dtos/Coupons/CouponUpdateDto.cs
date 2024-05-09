namespace Discount.API.Models.Dtos.Coupons
{
    public class CouponUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
