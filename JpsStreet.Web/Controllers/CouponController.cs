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

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTo>? list = new();

            ResponseDTo? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTo>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        // Function for routing or tracking the path or take path control for create cooupn
        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCouponCode(CouponDTo model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTo? response = await _couponService.CreateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }
            return View(model);
        }
    }
}