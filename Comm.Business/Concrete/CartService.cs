using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Concrete
{
    public class CartService : ICartService
    {
        private ICartRepository _cartRepository;
        private IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }



        public async Task InitializeCartAsync(string userId)
        {
            await _cartRepository.Create(new Cart() { UserId = userId });


        }


        public async Task AddToCart(int productId, string userId)
        {

            var product = await _productRepository.GetByIdAsync(productId);
            var cart = await GetCartByUserIdAsync(userId);

            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);
              

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = 1,
                        CartId = cart.Id,
                        Product = product
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity++;
                }

                await _cartRepository.Update(cart);
            }

        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<int> GetCountItemsAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            var count = 0;
            if (cart != null)
            {
                foreach (var item in cart.CartItems)
                {
                    count += item.Quantity;
                }
            }
          
      
            return count;
        }

        public async Task Update(Cart entity)
        {
            await _cartRepository.Update(entity);
        }

        public async Task RemoveFromCart(string userId,int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                _cartRepository.RemoveFromCart(cart.Id,productId);
            }
           

        }

        public async Task ClearCart(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            cart.CartItems.Clear();
            await _cartRepository.Update(cart);
        }
    }
}

