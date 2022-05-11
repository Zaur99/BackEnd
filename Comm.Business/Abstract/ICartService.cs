using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        void AddToCart(int productId,string userId);
        Cart GetCartByUserId(string userId);
        int GetCountItems(string userId);
        void Update(Cart entity);
        void RemoveFromCart(string userId,int productId);
        void ClearCart(int cartId);
    }
}
