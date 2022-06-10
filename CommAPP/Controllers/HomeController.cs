using Microsoft.AspNetCore.Mvc;
using Comm.Business.Abstract;
using CommAPP.Models.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {

            var categories =await  _categoryService.GetAllAsync();
            var products =await _productService.GetAllAsync(i => i.IsHome);

            var cateogoryVm = _mapper.Map<List<CategoryViewModel>>(categories);
            var productVm = _mapper.Map<List<ProductViewModel>>(products);
            

            ViewData["Categories"] = cateogoryVm;

            return View(productVm);
        }

       


    }
}
