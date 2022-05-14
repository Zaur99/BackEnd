using Comm.Business.Abstract;
using Comm.DataAccess;
using Comm.Entities;
using CommAPP.Models.ViewModels;
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
        private IProductService _productService;
        private ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;


        public AdminController(IProductService productService,
                                ICategoryService categoryService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;



        }
        public IActionResult AdminProducts()
        {
            return View(new ProductListViewModel
            {
                Products = _productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

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

                _productService.Create(entity);
                return RedirectToAction("AdminProducts");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

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

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds,IFormFile file)
        {
            if (ModelState.IsValid)
            {

                var entity = _productService.GetById(model.Id);

                string uniqueFileName = await UploadedFile(file);
                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Description = model.Description;
                entity.Url = model.Url;
                entity.IsHome = model.IsHome;
                entity.IsApproved = model.IsApproved;
                entity.ImageUrl = file == null ? model.ImageUrl : uniqueFileName;
               




                _productService.Update(entity, categoryIds);

                return RedirectToAction("AdminProducts");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }


        [HttpPost]

        public IActionResult DeleteProduct(int id)
        {
            var entity = _productService.GetById(id);

            if (entity == null)
            {
                return NotFound();

            }

            _productService.Delete(entity);

            return RedirectToAction("AdminProducts");
        }


        public IActionResult AdminCategories()
        {

            return View(new CategoryListViewModel
            {
                Categories = _categoryService.GetAll()

            });
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            var categories = _categoryService.GetAll();

            var vm = new CategoryListViewModel()
            {
                Categories = categories
            };

            ViewData["Categories"] = vm;

            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {

            if (ModelState.IsValid)
            {
                var entity = new Category
                {
                    Name = model.Name,
                    Url = model.Url,
                     
                };

                _categoryService.Create(entity);
                return RedirectToAction("AdminCategories");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _categoryService.GetByIdWithProducts((int)id);

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

        [HttpPost]

        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.GetByIdWithProducts((int)model.Id);

                category.Name = model.Name;
                category.Url = model.Url;
                _categoryService.Update(category);

                return RedirectToAction("AdminCategories");
            }
            return View(model);
        }


        [HttpPost]

        public IActionResult DeleteCategory(int id)
        {
            var entity = _categoryService.GetById(id);

            if (entity == null)
            {
                return NotFound();

            }

            _categoryService.Delete(entity);

            return RedirectToAction("AdminCategories");
        }

        [HttpPost]

        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("EditCategory/" + categoryId);
        }

        public async Task<string> UploadedFile(IFormFile file)
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
