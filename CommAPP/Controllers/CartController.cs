using AutoMapper;
using Comm.Business.Abstract;
using Comm.Entities;
using CommAPP.Models;
using CommAPP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CommAPP.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(IProductService productService, 
                              UserManager<ApplicationUser> userManager, 
                              ICartService cartService,
                              IMapper mapper)
        {
            _productService = productService;
            _userManager = userManager;
            _cartService = cartService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

               
                var cartVm = new CartModel()
                {
                    Id = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ImageUrl = i.Product.ImageUrl,
                        Name = i.Product.Name,
                        Price = i.Product.Price,
                        Quantity = i.Quantity,
                        Url = i.Product.Url,
                        Star = i.Product.Comments.Any() ? i.Product.Comments.Average(x => Convert.ToDouble(x.Star)) : 0

                    }).ToList()
                };

                return View(cartVm);
            }
            CartModel cartModel = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");

            return View(cartModel);
        }


        //Get Total Price dynamically by sending request (Ajax)
        public IActionResult GetTotalPrice()
        {
            if (User.Identity.IsAuthenticated)
            {
                var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

                //load price and quantity in order to calculate total price
                var model = new CartModel()
                {
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var json = JsonConvert.SerializeObject(model);

                return Json(json);
            }

            CartModel cartSession = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
            return Json(JsonConvert.SerializeObject(cartSession));

        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId);

            if (User.Identity.IsAuthenticated)
            {
                _cartService.AddToCart(productId, _userManager.GetUserId(User));
                return RedirectToAction("Index");
            }
            else
            {
                AddToCartSession(product, productId);
                return RedirectToAction("Index");
            }

        }

        //Save cart items in session if user is not authenticated
        public void AddToCartSession(Product product, int productId)
        {
            if (SessionHelper.GetObjectFromJson<Cart>(HttpContext.Session, "cart") == null)
            {
                var cart = new CartModel();

                cart.CartItems.Add(new CartItemModel()
                {
                    Id = product.Id,
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ProductId = product.Id,
                    Url = product.Url,
                    Star = product.Comments.Any() ? product.Comments.Average(x => Convert.ToDouble(x.Star)) : 0
                });

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            }
            else
            {
                CartModel cart = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
                int index = isExist(productId);
                if (index != -1)
                {
                    cart.CartItems[index].Quantity++;
                }
                else
                {
                    cart.CartItems.Add(new CartItemModel {
                        Id = product.Id, 
                        ImageUrl = product.ImageUrl,
                        Name = product.Name, Price = product.Price,
                        Quantity = 1,
                        ProductId = product.Id, 
                        Url = product.Url,
                        Star = product.Comments.Any() ? product.Comments.Average(x => Convert.ToDouble(x.Star)) : 0
                    });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);


            }
        }


        //Update value of item quantity in client-side
        public IActionResult AdjustValue(int id, int value)
        {
            if (User.Identity.IsAuthenticated)
            {
                var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
                var item = cart.CartItems.FirstOrDefault(i => i.Id == id);
                item.Quantity = value;
                _cartService.Update(cart);


                return Json("ok");
            }

            CartModel cartSession = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
            var itemSession = cartSession.CartItems.FirstOrDefault(i => i.Id == id);
            itemSession.Quantity = value;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cartSession);

            return Json("ok");


        }

        public IActionResult CountItems()
        {
            int count;
            if (User.Identity.IsAuthenticated)
            {
                count = _cartService.GetCountItems(_userManager.GetUserId(User));
            }
            else
            {

                CartModel cartSession = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
                count = cartSession.CartItems.Count();


            }

            return Json(new { success = true, message = count });
        }


       

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.RemoveFromCart(_userManager.GetUserId(User), productId);
                return RedirectToAction("Index");
            }
            else
            {
                CartModel cartModel = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");

                var item = cartModel.CartItems.FirstOrDefault(i => i.ProductId == productId);
                cartModel.CartItems.Remove(item);


                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cartModel);


                return RedirectToAction("Index");
            }


        }
        private int isExist(int id)
        {
            CartModel cart = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.CartItems.Count; i++)
            {
                if (cart.CartItems[i].ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
