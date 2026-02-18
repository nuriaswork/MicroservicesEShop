using System.ComponentModel.DataAnnotations;

namespace MsEShop.Services.CouponAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public double DiscountAmount { get; set; }
        
        public int MinAmount { get; set; }
        
        public DateTime LastUpdatedOn { get; set; }
        
        public DateTime CreatedOn { get; set; }
    }
}
