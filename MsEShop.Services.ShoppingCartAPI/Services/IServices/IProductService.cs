using MsEShop.Services.ShoppingCartAPI.Models.Dto;

namespace MsEShop.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
