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
        private ICategoryService _categoryService;
        public CategoryViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public IViewComponentResult Invoke()
        {
            string category = "";
            if (RouteData.Values["category"] != null)
            {
                category = RouteData.Values["category"].ToString();
            }
            var categories = _categoryService.GetAll();
            return View(new CategoryListViewModel {  Categories = categories, SelectedCategory =  category});
        }
    }
}
