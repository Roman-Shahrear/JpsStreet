using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace JpsStreet.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDToBasedOnLoggedInUser());
        }


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTo? response = await _shoppingCartService.RemoveFromShoppingCartAsync(cartDetailsId);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTo cartDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid coupon details.";
                return RedirectToAction(nameof(CartIndex));
            }
            ResponseDTo? response = await _shoppingCartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully.";
            }
            else
            {
                TempData["error"] = "Failed to apply coupon. Please check the code.";
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDTo cartDto)
        {
            CartDTo cart = await LoadCartDToBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDTo? response = await _shoppingCartService.EmailCart(cart);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTo cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDTo? response = await _shoppingCartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDTo> LoadCartDToBasedOnLoggedInUser()
        {
            //var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            ResponseDTo? response = await _shoppingCartService.GetShoppingCartByUserIDAsync(userId);
            if(response !=null & response.IsSuccess)
            {
                CartDTo cartDTo = JsonConvert.DeserializeObject<CartDTo>(Convert.ToString(response.Result));
                return cartDTo;
            }
            return new CartDTo();
        }
    }
}
