using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Abstract
{
    public interface IOrderRepository :IRepository<Order>
    {
         Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        
        
    }
}
