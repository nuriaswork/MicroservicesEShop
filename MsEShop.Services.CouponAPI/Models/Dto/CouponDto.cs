namespace MsEShop.Services.CouponAPI.Models.Dto
{
    //We'll be using NuGet Automapper to map between Coupon and CouponDto
    //Automapper >=13.0 does not need AutoMapper.Extensions.Microsoft.DependencyInjection any more.
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
