using AutoMapper;
using JpsStreet.Message.RabbiMQ;
using JpsStreet.Services.OrderApi.Data;
using JpsStreet.Services.OrderApi.Models;
using JpsStreet.Services.OrderApi.Models.DTo;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace JpsStreet.Services.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        protected ResponseDTo _response;
        private IMapper _mapper;
        private readonly OrderAppDbContext _db;
        private readonly IMessageRabbitMQ _messageRabbitMQ;
        private readonly IConfiguration _configuration;
        private IProductService _productService;

        public OrderApiController(OrderAppDbContext db, IProductService productService, IMapper mapper, IConfiguration configuration, IMessageRabbitMQ messageRabbitMQ)
        {
            _db = db;
            _messageRabbitMQ = messageRabbitMQ;
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
            _response = new ResponseDTo();
        }

        //[Authorize]
        [HttpPost("createOrder")]
        public async Task<ResponseDTo> CreateOrder([FromBody] CartDTo cartDTo)
        {
            try
            {
                OrderHeaderDTo orderHeaderDTo = _mapper.Map<OrderHeaderDTo>(cartDTo.CartHeader);
                orderHeaderDTo.OrderTime = DateTime.Now;
                orderHeaderDTo.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTo>>(cartDTo.CartDetails);
                orderHeaderDTo.OrderTotal = Math.Round(orderHeaderDTo.OrderTotal, 2);
                OrderHeader orederCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDTo)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDTo.OrderHeaderId = orederCreated.OrderHeaderId;
                _response.Result = orderHeaderDTo;
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
