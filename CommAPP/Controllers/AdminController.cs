using AutoMapper;
using Comm.Business.Abstract;
using Comm.DataAccess;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using CommAPP.Models.ViewModels.Admin;
using CommAPP.Models.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminController(IProductService productService,
                                ICategoryService categoryService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<IActionResult> CreateProduct(ProductModel model,IFormFile file)
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


        public async Task<IActionResult> AdminCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            var vm = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

            return View(vm);
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
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            var categories =await  _categoryService.GetAllAsync();


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
            var entity =await  _categoryService.GetByIdAsync(id);

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

        private async Task<string> UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" +file.FileName;


                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


            }

            return uniqueFileName;
        }
        

        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = model.RoleName });

                return RedirectToAction("RoleList");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();

            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }

            var model = new RoleDetailsModel
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers

            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> EditRole(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Xəta baş verdi...");
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Xəta baş verdi...");
                        }
                    }
                }
            }

            return RedirectToAction("RoleList", new { id = model.RoleId });
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users.ToList());
        }
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEditModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                SelectedRoles = await _userManager.GetRolesAsync(user)


            };

            ViewBag.Roles = _roleManager.Roles.Select(i => i.Name);
            return View(model);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return RedirectToAction("Account", "Login");
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.EmailConfirmed = model.EmailConfirmed;

                var result = await _userManager.UpdateAsync(user);
                //ViewBag.Roles = _roleManager.Roles.Select(i => i.Name);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    selectedRoles = selectedRoles ?? new string[] { };
                    await _userManager.AddToRolesAsync(user, selectedRoles.Except(roles).ToArray<string>());
                    await _userManager.RemoveFromRolesAsync(user, roles.Except(selectedRoles).ToArray<string>());

                    return RedirectToAction("UserList");

                }
                return RedirectToAction("UserList");

            }
            return View(model);
        }

    }
}
