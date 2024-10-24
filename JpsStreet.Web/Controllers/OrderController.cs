﻿using JpsStreet.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace JpsStreet.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
