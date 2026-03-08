using Microsoft.AspNetCore.Mvc;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;

namespace MsEShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
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

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _productService.CreateProductAsync(model);

                if (response != null && response.Success)
                {
                    TempData["success"] = "Product created!!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.Success)
            {
                ProductDto ProductDto = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
                return View(ProductDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto ProductDto)
        {
            ResponseDto responseDto = await _productService.DeleteProductAsync(ProductDto.ProductId);

            if (responseDto != null && responseDto.Success)
            {
                TempData["success"] = "Product deleted!!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }

            return View(ProductDto);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.Success)
            {
                ProductDto ProductDto = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
                return View(ProductDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _productService.UpdateProductAsync(model);

                if (response != null && response.Success)
                {
                    TempData["success"] = "Product modified!!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductView(int productId)
        {
            ResponseDto responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.Success)
            {
                ProductDto ProductDto = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
                return View(ProductDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return NotFound();
        }
    }
}
