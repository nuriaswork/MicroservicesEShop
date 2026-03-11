using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace MsEShop.Web.Controllers
{
    public class HomeController : BaseLoggedController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = [];

            ResponseDto response = await _productService.GetAllProductsAsync();

            if (response != null && response.Success)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ResponseDto response = await _productService.GetProductByIdAsync(productId);
            ProductDto model = new();

            if (response != null && response.Success)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartRequestDto cartRequestDto = new()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = LoggedUserId
                }
            };
            CartDetailsDto cartDetailsDto = new()
            {
                ProductId = productDto.ProductId,
                Count = productDto.Count
            };
            cartRequestDto.CartDetails = cartDetailsDto;

            ResponseDto response = await _cartService.UpsertCartAsync(cartRequestDto);
            if (response != null && response.Success)
            {
                TempData["success"] = "Product updated in shopping cart.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
