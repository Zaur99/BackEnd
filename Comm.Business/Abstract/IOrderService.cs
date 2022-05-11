using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface IOrderService 
    {
        IEnumerable<Order> GetAll(Expression<Func<Order, bool>> filter = null);

        Order GetById(int id);

        void Update(Order entity);
        void Delete(Order entity);
        void Create(Order entity);
        public List<Order> GetOrdersByUserId(string userId);
        public void PassCartToOrder(int orderId, int cartId);
    }
}
