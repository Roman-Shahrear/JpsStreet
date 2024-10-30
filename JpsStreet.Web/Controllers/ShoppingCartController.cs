using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JpsStreet.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService, ILogger<ShoppingCartController> logger)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            var cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {

            var cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }


        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDTo cartDto)
        {
            CartDTo cartDTo = await LoadCartDtoBasedOnLoggedInUser();

            if (cartDto == null || cartDTo == null)
            {
                TempData["error"] = "Cart data is not available. Please try again.";
                return RedirectToAction(nameof(CartIndex));
            }

            if (cartDTo.CartHeader == null)
            {
                TempData["error"] = "Cart header information is missing. Please check your cart.";
                return RedirectToAction(nameof(CartIndex));
            }

            cartDTo.CartHeader.Phone = cartDto.CartHeader?.Phone;
            cartDTo.CartHeader.Email = cartDto.CartHeader?.Email;
            cartDTo.CartHeader.Name = cartDto.CartHeader?.Name;

            _logger.LogInformation("Checkout initiated for User ID: {UserId}", User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            _logger.LogInformation("Cart Data: {@CartDTo}", cartDTo);

            var response = await _orderService.CreateOrder(cartDTo);
            _logger.LogInformation("Order Creation Response: {@Response}", response);

            if (response.IsSuccess && response.Result != null)
            {
                OrderHeaderDTo orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDTo>(response.Result.ToString());

                var domain = $"{Request.Scheme}://{Request.Host}/";
                var stripeRequestDto = new StripeRequestDTo
                {
                    ApprovedUrl = $"{domain}cart/Confirmation?orderId={orderHeaderDto.OrderHeaderId}",
                    CancelUrl = $"{domain}cart/checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                if (stripeResponse != null && stripeResponse.IsSuccess)
                {
                    var stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDTo>(stripeResponse.Result?.ToString());
                    if (stripeResponseResult?.StripeSessionUrl != null)
                    {
                        Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                        return new StatusCodeResult(303);
                    }
                }

                _logger.LogWarning("Stripe session creation failed: {@StripeResponse}", stripeResponse);
                TempData["error"] = "Failed to create Stripe session. Please try again.";
                return RedirectToAction(nameof(CartIndex));
            }

            _logger.LogWarning("Order creation failed with status: {StatusCode} and message: {Message}", response.StatusCode, response.ErrorMessage);
            TempData["error"] = "Order creation was unsuccessful. Please check your details.";
            return RedirectToAction(nameof(CartIndex));
        }



        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDTo? response = await _orderService.ValidateStripeSession(orderId);
            if (response != null & response.IsSuccess)
            {

                OrderHeaderDTo orderHeader = JsonConvert.DeserializeObject<OrderHeaderDTo>(Convert.ToString(response.Result));
                if (orderHeader.Status == SD.Status_Approved)
                {
                    return View(orderId);
                }
            }
            //redirect to some error page based on status
            return View(orderId);
        }
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTo? response = await _shoppingCartService.RemoveFromShoppingCartAsync(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTo cartDto)
        {

            ResponseDTo? response = await _shoppingCartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDTo cartDto)
        {
            CartDTo cart = await LoadCartDtoBasedOnLoggedInUser();
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

        private async Task<CartDTo> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTo? response = await _shoppingCartService.GetShoppingCartByUserIDAsync(userId);
            if (response != null & response.IsSuccess)
            {
                CartDTo cartDto = JsonConvert.DeserializeObject<CartDTo>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDTo();
        }
    }
}