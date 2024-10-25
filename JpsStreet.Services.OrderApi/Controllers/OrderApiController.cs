using AutoMapper;
using JpsStreet.Message.RabbiMQ;
using JpsStreet.Services.OrderApi.Data;
using JpsStreet.Services.OrderApi.Models;
using JpsStreet.Services.OrderApi.Models.DTo;
using JpsStreet.Services.OrderApi.Utility;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

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

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDTo> CreateOrder([FromBody] CartDTo cartDTo)
        {
            try
            {
                OrderHeaderDTo orderHeaderDTo = _mapper.Map<OrderHeaderDTo>(cartDTo.CartHeader);
                orderHeaderDTo.OrderTime = DateTime.Now;
                orderHeaderDTo.Status = SD.Status_Pending;
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

        [Authorize]
        [HttpGet("GetOrders")]
        public ResponseDTo GetOrders(string? userId = "")
        {
            try
            {
                List<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    objList = _db.OrderHeaders.Include(u => u.OrderDetails).OrderByDescending(u=>u.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _db.OrderHeaders.Include(u => u.OrderDetails).Where(u => u.UserId == userId).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDTo>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpGet("GetOrderById/{orderId:int}")]
        public ResponseDTo GetOrderById(int orderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.Include(u => u.OrderDetails).First(u => u.OrderHeaderId == orderId);
                _response.Result = _mapper.Map<OrderHeaderDTo>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDTo> CreateStripeSession([FromBody] StripeRequestDTo stripeRequestDTo)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDTo.ApproveUrl,
                    CancelUrl = stripeRequestDTo.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var DiscountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = stripeRequestDTo.OrderHeader.CouponCode
                    }
                };

                foreach (var item in stripeRequestDTo.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), //$30.99 -> 3099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDTo.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountObj;
                }
                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDTo.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == stripeRequestDTo.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _db.SaveChanges();
                _response.Result = stripeRequestDTo;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDTo> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status == "succeed")
                {
                    // then payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();
                    //RewardsDto rewardsDto = new()
                    //{
                    //    OrderId = orderHeader.OrderHeaderId,
                    //    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                    //    UserId = orderHeader.UserId
                    //};

                    //string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                    //await _messageRabbitMQ.PublishMessage(rewardsDTo, topicName);
                    _response.Result = _mapper.Map<OrderHeaderDTo>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        //[Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDTo> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderId);
                if(orderHeader != null)
                {
                    if(newStatus == SD.Status_Cancelled)
                    {
                        // We will give refund
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                    orderHeader.Status = newStatus;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
