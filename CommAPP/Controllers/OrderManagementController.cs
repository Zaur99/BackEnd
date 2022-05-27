using AutoMapper;
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
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderManagementController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var orders = _orderService.GetAll();

            var vm = _mapper.Map<IEnumerable<OrderViewModel>>(orders);

            return View(vm);

        }

        [HttpGet]
        public IActionResult Edit(int orderId)
        {
            var order = _orderService.GetById(orderId);

            var vm = _mapper.Map<OrderFullViewModel>(order);

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
