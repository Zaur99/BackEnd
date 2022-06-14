using AutoMapper;
using Comm.Business.Abstract;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using CommAPP.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryManagementController(ICategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> AdminCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            var vm = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

            return View(vm);
        }
       
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            var categories = await _categoryService.GetAllAsync();


            var categoryItems = new List<NestedCategoriesViewModel>();

            var topCategories = categories.Where(x => !x.ParentId.HasValue);

            foreach (var category in topCategories)
            {
                var categoryMenuItem = Map(category);
                categoryItems.Add(categoryMenuItem);
            }

            ViewData["Categories"] = categoryItems;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> CreateCategory(CategoryModel model)
        {

            if (ModelState.IsValid)
            {
                var entity = new Category
                {
                    Name = model.Name,
                    Url = model.Url,
                    ParentId = model.ParentId
                };

                await _categoryService.Create(entity);
                return RedirectToAction("AdminCategories");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _categoryService.GetByIdAsync((int)id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                Products = category.ProductCategories.Select(i => i.Product).ToList()
            };


            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryService.GetByIdAsync((int)model.Id);

                category.Name = model.Name;
                category.Url = model.Url;
                await _categoryService.Update(category);

                return RedirectToAction("AdminCategories");
            }
            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var entity = await _categoryService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();

            }

            await _categoryService.Delete(entity);

            return RedirectToAction("AdminCategories");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("EditCategory/" + categoryId);
        }


        private NestedCategoriesViewModel Map(Category category)
        {
            var categoryMenuItem = new NestedCategoriesViewModel
            {
                Id = category.Id,
                Name = category.Name,
            };

            var childCategories = category.Children;
            foreach (var childCategory in childCategories)
            {
                var childCategoryMenuItem = Map(childCategory);
                categoryMenuItem.AddChildItem(childCategoryMenuItem);
            }

            return categoryMenuItem;
        }
    }
}
