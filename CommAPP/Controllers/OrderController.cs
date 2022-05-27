using Comm.Business.Abstract;
using Comm.Entities;
using CommAPP.Models.ViewModels;
using CommAPP.Models.ViewModels.OrderRelated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        private UserManager<ApplicationUser> _userManager;
        private ICartService _cartService;
        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager, ICartService cartService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                var orders = _orderService.GetOrdersByUserId(_userManager.GetUserId(User));

                var vm = new OrderListViewModel()
                {
                    OrderListVM = orders.Select(i => new OrderViewModel()
                    {
                        OrderStatus = i.OrderStatus,
                        OrderItemListVM = i.OrderItems.Select(x => new OrderItemModel() { Product = x.Product })
                    })

                };

                return View(vm);
            }
            return View();
        }




        public IActionResult Checkout()
        {

            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var vm = new CartModel()
            {
                 
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    ImageUrl = i.Product.ImageUrl,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()

            };

            ViewData["Cart"] = vm;

            return View();

        }

        [HttpPost]
        public  IActionResult CompleteOrder(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                var order = new Order();

                order.FullName = model.FullName;
                order.Email = model.Email;
                order.Adress = model.Adress;
                order.City = model.City;
                order.OrderedTime = DateTime.Now;
                order.ExtraDetails = model.ExtraDetails;
                order.TotalPrice = model.TotalPrice;
                order.PhoneNumber = model.PhoneNumber;
                order.OrderStatus = "Pending";
                order.UserId = userId;

                _orderService.Create(order);

                //Pass Cart to Order, then clear Cart
                _orderService.PassCartToOrder(order.Id, cart.Id);
                _cartService.ClearCart(cart.Id);




                return RedirectToAction("Index");

            }
            return View();
        }
    }
}
