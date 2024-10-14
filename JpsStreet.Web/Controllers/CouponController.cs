using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace JpsStreet.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        // For retreive data we need to get all data using deserialized from database to view in frontend all list data 
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTo>? list = new();

            ResponseDTo? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTo>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        // Function for routing or tracking the path or take path control for create coupon
        [HttpGet]
        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDTo model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTo? response = await _couponService.CreateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        // Function for routing or tracking the path or take path control for single delete cooupn code and also need to deserialized for view in frontend
        [HttpGet]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            ResponseDTo? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDTo? model = JsonConvert.DeserializeObject<CouponDTo>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(CouponDTo couponDTo)
        {
            ResponseDTo? response = await _couponService.DeleteCouponsAsync(couponDTo.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDTo);
        }
    }
}