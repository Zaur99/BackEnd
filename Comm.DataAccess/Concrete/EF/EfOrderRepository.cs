using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfOrderRepository :EfRepository<Order>, IOrderRepository
    {
        protected new readonly CommerceContext context;

        public EfOrderRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }

        public List<Order> GetOrdersByUserId(string userId)
        {
            return context.Orders.Where(i => i.UserId == userId).Include(i => i.OrderItems).ThenInclude(i=>i.Product).ToList();
        }
    }
}
