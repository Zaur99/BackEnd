using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.DataAccess.Concrete.EF;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Concrete
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _repository;
        private ICartRepository _cartRepository;
        public OrderService(IOrderRepository repository, ICartRepository cartRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
        }

       

        public void Create(Order entity)
        {
            _repository.Create(entity);
        }

        public void Delete(Order entity)
        {
            _repository.Delete(entity);
        }

        public IEnumerable<Order> GetAll(Expression<Func<Order, bool>> filter = null)
        {
            return _repository.GetAll(filter);
        }

        public Order GetById(int id)
        {
            return _repository.GetById(id);

        }

        public List<Order> GetOrdersByUserId(string userId)
        {
            return _repository.GetOrdersByUserId(userId);
        }

        public void PassCartToOrder(int orderId, int cartId)
        {
            var order = _repository.GetById(orderId);
            var cart = _cartRepository.GetById(cartId);

            order.OrderItems = cart.CartItems.Select(i => new OrderItem()
            { OrderId = order.Id, Product = i.Product, ProductId = i.ProductId, Order = order, Quantity = i.Quantity }).ToList();
            

            _repository.Update(order);
        }

        public void Update(Order entity)
        {
            _repository.Update(entity);
        }
    }
}
