using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfCartRepository : EfRepository<Cart>, ICartRepository
    {
        protected new readonly CommerceContext context;

        public EfCartRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }

      

        public  async Task<Cart> GetCartByUserIdAsync(string userId)
        {

            return await context.Carts.FirstOrDefaultAsync(i => i.UserId == userId);


        }

        public void RemoveFromCart(int cartId, int productId)
        {


            var cmd = @"Delete From CartItems where CartId=@p0 AND ProductId=@p1";

            context.Database.ExecuteSqlRaw(cmd, cartId, productId);




        }

        public async override Task Update(Cart entity)
        {

            context.Carts.Update(entity);
            await context.SaveChangesAsync();

        }
    }
}
