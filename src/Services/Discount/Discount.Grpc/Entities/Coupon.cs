namespace Discount.Grpc.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
