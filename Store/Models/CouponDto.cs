using System.ComponentModel;

namespace Store.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        [DisplayName("Coupon Code :")]
        public string CouponCode { get; set; }
        [DisplayName("Discount Amount :")]
        public double DiscountAmount { get; set; }
        [DisplayName("Min Amount :")]
        public int MinAmount { get; set; }
    }
}
