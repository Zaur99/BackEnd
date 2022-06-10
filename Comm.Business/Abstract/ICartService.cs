using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Abstract
{
    public interface ICartService
    {
        Task InitializeCartAsync(string userId);
        Task AddToCart(int productId,string userId);
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<int> GetCountItemsAsync(string userId);
        Task Update(Cart entity);
        Task RemoveFromCart(string userId,int productId);
        Task ClearCart(int cartId);
    }
}
