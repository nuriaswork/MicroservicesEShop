using MsEShop.Web.Enums;

namespace MsEShop.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public string Data { get; set; }
    }
}
