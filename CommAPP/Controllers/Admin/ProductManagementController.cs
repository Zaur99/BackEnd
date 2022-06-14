using AutoMapper;
using Comm.Business.Abstract;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;



        public ProductManagementController(IProductService productService,
                                           ICategoryService categoryService,
                                           IWebHostEnvironment webHostEnvironment,
                                           IMapper mapper
                                          )
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;


        }
        public async Task<IActionResult> AdminProducts()
        {
            var products = await _productService.GetAllAsync();
            var vm = _mapper.Map<List<ProductViewModel>>(products);

            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> CreateProduct(ProductModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = await UploadedFile(file);
                var entity = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    Url = model.Url,
                    ImageUrl = uniqueFileName

                };

                await _productService.Create(entity);
                return RedirectToAction("AdminProducts");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _productService.GetByIdWithCategoriesAsync((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                Price = entity.Price,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            var vm = new ProductEditViewModel
            {
                ProductEditModel = model,

            };


            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductEditViewModel model, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {

                var entity = _productService.GetByIdAsync(model.ProductEditModel.Id);

                string uniqueFileName = await UploadedFile(model.File);

                var entityResult = await entity;

                entityResult.Name = model.ProductEditModel.Name;
                entityResult.Price = model.ProductEditModel.Price;
                entityResult.Description = model.ProductEditModel.Description;
                entityResult.Url = model.ProductEditModel.Url;
                entityResult.IsHome = model.ProductEditModel.IsHome;
                entityResult.IsApproved = model.ProductEditModel.IsApproved;
                entityResult.ImageUrl = model.File == null ? model.ProductEditModel.ImageUrl : uniqueFileName;


                await _productService.Update(entityResult, categoryIds);

                return RedirectToAction("AdminProducts");
            }

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var entity = await _productService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();

            }

            await _productService.Delete(entity);

            return RedirectToAction("AdminProducts");
        }


        private async Task<string> UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;


                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


            }

            return uniqueFileName;
        }
    }
}
