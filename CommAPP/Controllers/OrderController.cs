using AutoMapper;
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
        private IMapper _mapper;
        public OrderController(IOrderService orderService,
                               UserManager<ApplicationUser> userManager,
                               ICartService cartService,
                               IMapper mapper)
        {
            _orderService = orderService;
            _userManager = userManager;
            _cartService = cartService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                var orders =await _orderService.GetOrdersByUserIdAsync(_userManager.GetUserId(User));

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




        public async Task<IActionResult> Checkout()
        {

            var cart = await _cartService.GetCartByUserIdAsync(_userManager.GetUserId(User));

            var testVm = _mapper.Map<CartModel>(cart);
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
        public async Task<IActionResult> CompleteOrder(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                var cart =  _cartService.GetCartByUserIdAsync(userId);

                var order = _mapper.Map<Order>(model);
                order.UserId = userId;
                order.OrderStatus = "Pending";

                await _orderService.Create(order);

                //Pass Cart to Order, then clear Cart
                await _orderService.PassCartToOrder(order.Id, cart.Result.Id);
                await _cartService.ClearCart(cart.Result.Id);

                return RedirectToAction("Index");

            }

            return View();
        }
    }
}
