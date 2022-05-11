using Comm.Business.Abstract;
using Comm.DataAccess;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productManager;
        private readonly ICategoryService _categoryManager;
        private readonly ICommentService _commentManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(IProductService productManager,
                              ICategoryService categoryManager,
                              ICommentService commentManager,
                              UserManager<ApplicationUser> userManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _commentManager = commentManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Details(string productname)
        {
            if (productname == null)
            {
                return NotFound();
            }
            Product product = _productManager.GetProductDetails(productname);

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                //Find if user has comment to related Product
                bool isMatched = _commentManager.FindMatching(product, user);


                //If User has comment , cannot write second Comment
                if (isMatched)
                    ViewBag.HasComment = true;
                else
                    ViewBag.HasComment = false;
            }

            if (product == null)
            {
                return NotFound();
            }

            var vm = product.Comments.Select(i => new CommentListViewModel
            {
                Star = i.Star,
                Text = i.Text,
                UserName = i.User.UserName
            });

            return View(new ProductDetailsViewModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList(),
                CommentListVM = vm
            });

        }

        [HttpPost]
        public async Task<IActionResult> SendComment(ProductDetailsViewModel model, string userName)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(userName);   
                
                var comment = new Comment();
                
                comment.ProductId = model.Product.Id;
                comment.Star = model.CommentVM.Star;
                comment.Text = model.CommentVM.Text;
                comment.User = user;
                comment.User.UserName = userName;
                comment.UserId = user.Id;

                _commentManager.Create(comment);
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Details", "Shop", new { productname = model.Product.Url });

            }

            return View(model);
        }

        static CategoryListViewModel _model;
        public IActionResult GetProductsByCategory(string category, int page = 1)
        {
            const int pageSize = 3;

            var a = _categoryManager.GetAllWithSubCategories(i => i.Name == category);


            if (_model == null || a.First().ParentId == null)
            {
                _model = new CategoryListViewModel()
                {
                    Categories = _categoryManager.GetAllWithSubCategories(i => i.Name == category),


                };
            }
            _model.SelectedCategory = category;
            TempData["Categories"] = _model;
            return View(new ProductListViewModel
            {
                PageDetails = new PageDetails()
                {
                    PageSize = pageSize,
                    TotalItems = _productManager.GetCountByCategory(category),
                    CurrentCategory = category,
                    CurrentPage = page
                },

                Products = _productManager.GetProductsByCategory(category, page, pageSize)
            });

        }
    }

}


