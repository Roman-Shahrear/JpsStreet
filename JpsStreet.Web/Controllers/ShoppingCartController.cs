using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace JpsStreet.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDtoBasedOnLoggedInUser());
        }


        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDTo cartDTo)
        {
            if (!ModelState.IsValid)
            {
                return View(await LoadCartDtoBasedOnLoggedInUser());
            }

            CartDTo cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDTo.CartHeader.Phone;
            cart.CartHeader.Email = cartDTo.CartHeader.Email;
            cart.CartHeader.Name = cartDTo.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            if (response == null || !response.IsSuccess)
            {
                // Log the error response
                ModelState.AddModelError(string.Empty, "Error creating order.");
                return View(await LoadCartDtoBasedOnLoggedInUser());
            }

            OrderHeaderDTo orderHeaderDTo = JsonConvert.DeserializeObject<OrderHeaderDTo>(Convert.ToString(response.Result));
            if (orderHeaderDTo == null)
            {
                // Handle the null case
                ModelState.AddModelError(string.Empty, "Failed to process the order.");
                return View(await LoadCartDtoBasedOnLoggedInUser());
            }
            //get stripe session and redirect to stripe to place order
            //
            var domain = $"{Request.Scheme}://{Request.Host.Value}/";
            StripeRequestDTo stripeRequestDTo = new()
            {
                ApprovedUrl = $"{domain}cart/Confirmation?orderId={orderHeaderDTo.OrderHeaderId}",
                CancelUrl = $"{domain}cart/checkout",
                OrderHeader = orderHeaderDTo
            };

            var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDTo);
            if (stripeResponse == null || !stripeResponse.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Failed to create a Stripe session.");
                return View(await LoadCartDtoBasedOnLoggedInUser());
            }

            StripeRequestDTo stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDTo>(Convert.ToString(stripeResponse.Result));
            if (string.IsNullOrEmpty(stripeResponseResult?.StripeSessionUrl))
            {
                ModelState.AddModelError(string.Empty, "Failed to get Stripe session URL.");
                return View(await LoadCartDtoBasedOnLoggedInUser());
            }

            Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
            return new StatusCodeResult(303);
        }

        //[HttpPost]
        //[ActionName("Checkout")]
        //public async Task<IActionResult> Checkout(CartDTo cartDto)
        //{

        //    CartDTo cart = await LoadCartDtoBasedOnLoggedInUser();
        //    cart.CartHeader.Phone = cartDto.CartHeader.Phone;
        //    cart.CartHeader.Email = cartDto.CartHeader.Email;
        //    cart.CartHeader.Name = cartDto.CartHeader.Name;

        //    var response = await _orderService.CreateOrder(cart);
        //    OrderHeaderDTo orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDTo>(Convert.ToString(response.Result));

        //    if (response != null && response.IsSuccess)
        //    {
        //        //get stripe session and redirect to stripe to place order
        //        //
        //        var domain = $"{Request.Scheme}://{Request.Host.Value}/";

        //        StripeRequestDTo stripeRequestDto = new()
        //        {
        //            ApprovedUrl = $"{domain}cart/Confirmation?orderId={orderHeaderDto.OrderHeaderId}",
        //            CancelUrl = $"{domain}cart/checkout",
        //            OrderHeader = orderHeaderDto
        //        };

        //        var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
        //        StripeRequestDTo stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDTo>
        //                                    (Convert.ToString(stripeResponse.Result));
        //        Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
        //        return new StatusCodeResult(303);



        //    }
        //    return View();
        //}


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
