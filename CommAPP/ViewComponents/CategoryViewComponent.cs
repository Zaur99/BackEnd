using AutoMapper;
using Comm.Business.Abstract;
using CommAPP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryViewComponent(ICategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        public IViewComponentResult Invoke()
        {
            string category = "";
            if (RouteData.Values["category"] != null)
            {
                category = RouteData.Values["category"].ToString();
            }
            var categories = _categoryService.GetAll();
            var vm = _mapper.Map<List<CategoryViewModel>>(categories);
            return View(vm);
        }
    }
}
