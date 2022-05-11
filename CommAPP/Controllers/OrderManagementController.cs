using Comm.Business.Abstract;
using CommAPP.Models.ViewModels.Admin;
using CommAPP.Models.ViewModels.OrderRelated;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    public class OrderManagementController : Controller
    {
        private IOrderService _orderService;
        public OrderManagementController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var orders = _orderService.GetAll();

            var vm = new OrderListViewModel()
            {
                OrderListVM = orders.Select(i => new OrderViewModel()
                {
                    OrderId = i.Id,
                    OrderStatus = i.OrderStatus,
                    OrderedTime = i.OrderedTime

                })

            };

            return View(vm);

        }

        [HttpGet]
        public IActionResult Edit(int orderId)
        {
            var order = _orderService.GetById(orderId);

            var vm = new OrderFullViewModel()
            {
                Id = order.Id,
                FullName = order.FullName,
                Email = order.Email,
                Adress = order.Adress,
                City = order.City,
                ExtraDetails = order.ExtraDetails,
                PhoneNumber = order.PhoneNumber,
                UserId = order.UserId,
                User = order.User,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                OrderedTime = order.OrderedTime,
                OrderItems = order.OrderItems.Select(x => new OrderItemModel() { Product = x.Product, Quantity = x.Quantity })


            };


            return View(vm);

        }
        [HttpPost]
        public IActionResult Edit(OrderFullViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _orderService.GetById(model.Id);

                order.OrderStatus = model.OrderStatus;

                _orderService.Update(order);
            }
            return RedirectToAction("Index");
        }
    }
}
