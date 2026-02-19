using MsEShop.Web.Enums;

namespace MsEShop.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; }
        public string Uri { get; set; }
        public object Data { get; set; }
    }
}
