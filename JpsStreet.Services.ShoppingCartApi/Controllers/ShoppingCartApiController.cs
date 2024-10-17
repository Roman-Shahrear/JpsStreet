using AutoMapper;
using JpsStreet.Services.ShoppingCartApi.Data;
using JpsStreet.Services.ShoppingCartApi.Models;
using JpsStreet.Services.ShoppingCartApi.Models.DTo;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace JpsStreet.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartApiController : ControllerBase
    {
        private readonly ShoppingCartAppDbContext _db;
        private ResponseDTo _response;
        private IMapper _mapper;
        private IProductService _productService;

        public ShoppingCartApiController(ShoppingCartAppDbContext db, IMapper mapper, IProductService productService)
        {
            _db = db;
            _productService = productService;
            _response = new ResponseDTo();
            _mapper = mapper;
        }

        [HttpGet("getCart/{userId}")]
        public async Task<ResponseDTo> GetCart(string userId)
        {
            try
            {
                CartDTo cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTo>(_db.CartHeaders.First(u => u.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTo>>(_db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));
                IEnumerable<ProductDTo> productDTos = await _productService.GetProducts();
                foreach(var item in cart.CartDetails)
                {
                    item.Product = productDTos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("cartUpsert")]
        public async Task<ResponseDTo> CartUpsert(CartDTo cartDTo)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u=>u.UserId == cartDTo.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    // create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTo.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDTo.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTo.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // if header is not null
                    // check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.FirstOrDefaultAsync(
                        u => u.ProductId == cartDTo.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if(cartDetailsFromDb == null)
                    {
                        // create cartdetails
                        cartDTo.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTo.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // update count in cart details
                        cartDTo.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTo.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTo.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDTo.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDTo;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("removeCart")]
        public async Task<ResponseDTo> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int totalCountOfCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if(totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
