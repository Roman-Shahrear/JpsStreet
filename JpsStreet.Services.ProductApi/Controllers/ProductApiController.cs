using AutoMapper;
using JpsStreet.Services.ProductApi.Data;
using JpsStreet.Services.ProductApi.Models;
using JpsStreet.Services.ProductApi.Models.DTo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JpsStreet.Services.ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductApiController : Controller
    {
        private readonly ProductAppDbContext _db;
        private IMapper _mapper;
        private ResponseDTo _response;

        public ProductApiController(ProductAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTo();
        }
        [HttpGet]
        public async Task<ResponseDTo> GetAllAsync()
        {
            try
            {
                IEnumerable<Product> objList = await _db.Products.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDTo>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseDTo> GetAsync(int id)
        {
            try
            {
                Product obj = await _db.Products.FirstAsync(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDTo>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTo> CreateCouponCodeAsync([FromBody] ProductDTo productDTo)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDTo);
                await _db.Products.AddAsync(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDTo>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTo> UpdateCouponCodeAsync([FromBody] ProductDTo productDTo)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDTo);
                _db.Products.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDTo>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTo> DeleteCouponCodeAsync(int id)
        {
            try
            {
                Product obj = await _db.Products.FirstAsync(u => u.ProductId == id);
                _db.Products.Remove(obj);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
