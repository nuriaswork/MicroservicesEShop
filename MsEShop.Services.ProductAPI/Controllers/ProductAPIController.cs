using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEShop.Services.ProductAPI.Data;
using MsEShop.Services.ProductAPI.Models;
using MsEShop.Services.ProductAPI.Models.Dto;

namespace MsEShop.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public ProductAPIController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _responseDto = new ResponseDto() { Success = true };
        }

        // GET: api/ProductAPI
        [HttpGet]
        public ResponseDto GetProducts()
        {
            try
            {
                IEnumerable<Product> products = _context.Products.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception e)
            {
                _responseDto.Result = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        // GET: api/ProductAPI/5
        [HttpGet("{id}")]
        public ResponseDto GetProduct(int id)
        {
            try
            {
                Product product = _context.Products.First(p => p.ProductId == id);
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDto GetByCode(string name)
        {
            try
            {
                Product product = _context.Products.First(p => p.Name == name);
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        // PUT: api/ProductAPI/5
        [HttpPut]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ResponseDto PutProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Update(product);
                _context.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        // POST: api/ProductAPI
        [HttpPost]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ResponseDto PostProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Products.Add(product);
                _context.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        // DELETE: api/ProductAPI/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                Product product = _context.Products.First(p => p.ProductId == id);
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }
    }
}
