using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Abstract
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> filter = null);

        Task<Order> GetByIdAsync(int id);

        Task Update(Order entity);
        Task Delete(Order entity);
        Task Create(Order entity);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task PassCartToOrder(int orderId, int cartId);
    }
}
