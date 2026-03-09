using MsEShop.Services.ShoppingCartAPI.Models.Dto;
using MsEShop.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace MsEShop.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("ProductHttpClient"); //defined in Program.cs
            HttpResponseMessage response = await client.GetAsync("/api/product");
            string content = await response.Content.ReadAsStringAsync();
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto != null && responseDto.Success)
            {
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseDto.Result.ToString());
                return products;
            }

            return new List<ProductDto>();
        }
    }
}
