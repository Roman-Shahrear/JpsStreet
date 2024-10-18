using AutoMapper;
using JpsStreet.Services.CouponApi.Data;
using JpsStreet.Services.CouponApi.Models.DTo;
using JpsStreet.Services.CouponApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JpsStreetServices.CouponApi.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponApiController : ControllerBase
    {
        private readonly CouponAppDbContext _db;
        private IMapper _mapper;
        private ResponseDTo _response;

        public CouponApiController(CouponAppDbContext db, IMapper mapper)
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
                IEnumerable<Coupon> objList = await _db.Coupons.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<CouponDTo>>(objList);
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
                Coupon obj = await _db.Coupons.FirstAsync(u => u.CouponId == id);
                _response.Result = _mapper.Map<CouponDTo>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("getByCode/{code}")]
        public async Task<ResponseDTo> GetByCodeAsync(string code)
        {
            try
            {
                Coupon obj = await _db.Coupons.FirstAsync(u => u.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDTo>(obj);
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
        public async Task<ResponseDTo> CreateCouponCodeAsync([FromBody] CouponDTo couponDTo)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTo);
                await _db.Coupons.AddAsync(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CouponDTo>(obj);
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
        public async Task<ResponseDTo> UpdateCouponCodeAsync([FromBody] CouponDTo couponDTo)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTo);
                _db.Coupons.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CouponDTo>(obj);
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
                Coupon obj = await _db.Coupons.FirstAsync(u => u.CouponId == id);
                _db.Coupons.Remove(obj);
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
