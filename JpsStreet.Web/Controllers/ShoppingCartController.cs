using Microsoft.AspNetCore.Mvc;

namespace JpsStreet.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
