using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        void RemoveFromCart(int cartId,int productId);
       
    }
}
