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
        public  async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();

            var vm = _mapper.Map<IEnumerable<OrderViewModel>>(orders);

            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);

            var vm = _mapper.Map<OrderFullViewModel>(order);

            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderFullViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order =await _orderService.GetByIdAsync(model.Id);

                order.OrderStatus = model.OrderStatus;

                await _orderService.Update(order);
            }
            return RedirectToAction("Index");
        }
    }
}
