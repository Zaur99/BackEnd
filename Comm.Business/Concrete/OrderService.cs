using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.DataAccess.Concrete.EF;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Concrete
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private ICartRepository _cartRepository;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

       

        public async Task Create(Order entity)
        {
            await _orderRepository.Create(entity);
        }

        public async Task Delete(Order entity)
        {
            await _orderRepository.Delete(entity);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> filter = null)
        {
            return await _orderRepository.GetAllAsync(filter);
        }

        public async  Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);

        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task  PassCartToOrder(int orderId, int cartId)
        {
            var order =await _orderRepository.GetByIdAsync(orderId);
            var cart = await _cartRepository.GetByIdAsync(cartId);

            order.OrderItems = cart.CartItems.Select(i => new OrderItem()
            { OrderId = order.Id, Product = i.Product, ProductId = i.ProductId, Order = order, Quantity = i.Quantity }).ToList();


            await _orderRepository.Update(order);
        }

        public async Task Update(Order entity)
        {
            await _orderRepository.Update(entity);
        }
    }
}
