using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.DataAccess.Abstract
{
    public interface IOrderRepository :IRepository<Order>
    {
        public List<Order> GetOrdersByUserId(string userId);
        
        
    }
}
