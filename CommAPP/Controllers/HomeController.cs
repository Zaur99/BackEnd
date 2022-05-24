using Microsoft.AspNetCore.Mvc;
using Comm.Business.Abstract;
using CommAPP.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace CommAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public HomeController(IProductService productService,ICategoryService categoryService,IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            var categories = _categoryService.GetAll();
            var products = _productService.GetAll(i => i.IsHome);

            var cateogoryVm = _mapper.Map<List<CategoryViewModel>>(categories.ToList());
            var productVm = _mapper.Map<List<ProductViewModel>>(products.ToList());
            //var vm = new CategoryListViewModel() {
            //   Categories =   categories
            //};

            ViewData["Categories"] = cateogoryVm;

            return View(productVm);
        }

       


    }
}
