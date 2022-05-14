using Microsoft.AspNetCore.Mvc;
using Comm.Business.Abstract;
using CommAPP.Models.ViewModels;

namespace CommAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {

            var categories = _categoryService.GetAll();

            var vm = new CategoryListViewModel() {
               Categories =   categories
            };

            ViewData["Categories"] = vm;

            return View(new ProductListViewModel
            {
                Products = _productService.GetAll(i=>i.IsHome)

            });
        }

       


    }
}
