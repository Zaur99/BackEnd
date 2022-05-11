using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.DataAccess.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
        Cart GetCartByUserId(string userId);
        void RemoveFromCart(int cartId,int productId);
       
    }
}
