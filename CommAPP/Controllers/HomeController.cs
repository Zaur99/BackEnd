using Microsoft.AspNetCore.Mvc;
using Comm.Business.Abstract;
using CommAPP.Models.ViewModels;

namespace CommAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productManager;

        public HomeController(IProductService productManager)
        {
            _productManager = productManager;
        }
        public IActionResult Index()
        {
            return View(new ProductListViewModel
            {
                Products = _productManager.GetHomeProducts()

            });
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
