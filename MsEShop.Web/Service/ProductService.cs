using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Service
{
    public class ProductService : IProductService
    {
        private const string ApiControllerName = "/api/Product";
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.POST,
                Data = ProductDto
            }, true);
        }

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName + $"/{id}",
                ApiType = Enums.ApiType.DELETE
            }, true);
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> GetProductAsync(string name)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName + $"/GetByName/{name}",
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName + $"/{id}",
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ProductAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.PUT,
                Data = ProductDto
            }, true);
        }
    }
}

