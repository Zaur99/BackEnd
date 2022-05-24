using AutoMapper;
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
        private readonly IMapper _mapper;
        static CategoryViewModel _model;


        public ShopController(IProductService productManager,
                              ICategoryService categoryManager,
                              ICommentService commentManager,
                              UserManager<ApplicationUser> userManager,
                              IMapper mapper)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _commentManager = commentManager;
            _userManager = userManager;
            _mapper = mapper;
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


        //Path : products/{category}
        public IActionResult GetProductsByCategory(string category, int page = 1)
        {
            const int pageSize = 3;

            IEnumerable<Product> products;
            IEnumerable<CategoryViewModel> categoryVm;

            if (category == null)
            {

                var categories = _categoryManager.GetAll(i => i.ParentId == null).ToList();


                categoryVm = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
                
                products = _productManager.GetApprovedProductsForPage(page, pageSize);

                ViewData["Categories"] = categoryVm;


                return View(new ProductListViewModel
                {

                    Products = products.Select(i => new ProductViewModel()
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        Name = i.Name,
                        Price = i.Price,
                        Url = i.Url
                    }),
                    PageDetails = new PageDetails()
                    {
                        PageSize = pageSize,
                        TotalItems = _productManager.GetAll(i => i.IsApproved).Count(),
                        CurrentPage = page
                    },
                });


            }

            var cat = _categoryManager.GetAll(i => i.Url == category).FirstOrDefault();


            //Get parent category if user click child category and create only one instance 
            if (_model == null || cat.Parent == null)
            {
                _model = _mapper.Map<CategoryViewModel>(cat);

            }

           
            ViewData["Category"] = _model;

            //Get products under particular category per page
            products = _productManager.GetProductsByCategory(category, page, pageSize);

            var productVm = new ProductListViewModel();

            productVm.Products = _mapper.Map<List<ProductViewModel>>(products.ToList());

            productVm.PageDetails = new PageDetails()
            {
                PageSize = pageSize,
                TotalItems = _productManager.GetCountByCategory(category),
                CurrentCategory = category,
                CurrentPage = page
            };

            return View(productVm);

        }


        public IActionResult Search(string searchString, int page = 1)
        {
            const int pageSize = 3;

            if (searchString == null)
            {
                return RedirectToAction("GetProductsByCategory");
            }

            //Get filtered Products
            var filteredProducts = _productManager.GetAll(i => i.Name.Contains(searchString));

            var products = _productManager.GetFilteredProductsForPage(searchString, page, pageSize);

            ViewBag.SearchString = searchString;


            var productVm = new ProductListViewModel();

            productVm.Products = _mapper.Map<List<ProductViewModel>>(products.ToList());
            productVm.PageDetails = new PageDetails()
            {
                PageSize = pageSize,
                TotalItems = filteredProducts.Count(),
                CurrentPage = page
            };

            return View(productVm);
        }
    }

}


