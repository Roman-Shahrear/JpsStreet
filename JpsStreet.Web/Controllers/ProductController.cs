using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace JpsStreet.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        // For retreive data we need to get all data using deserialized from database to view in frontend all list data 
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTo>? list = new();

            ResponseDTo? response = await _ProductService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTo>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        // Function for routing or tracking the path or take path control for create Product
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTo model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTo? response = await _ProductService.CreateProductsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
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
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            ResponseDTo? response = await _ProductService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTo? model = JsonConvert.DeserializeObject<ProductDTo>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDTo productDTo)
        {
            ResponseDTo? response = await _ProductService.DeleteProductsAsync(productDTo.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDTo);
        }
    }
}