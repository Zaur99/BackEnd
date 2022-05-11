using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comm.Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        private IProductRepository _productRepository;

        public CartManager(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }



        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart() { UserId = userId });


        }


        public void AddToCart(int productId, string userId)
        {

            var product = _productRepository.GetById(productId);


            var cart = GetCartByUserId(userId);

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

                _cartRepository.Update(cart);
            }

        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetCartByUserId(userId);
        }

        public int GetCountItems(string userId)
        {
            var cart = _cartRepository.GetCartByUserId(userId);
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

        public void Update(Cart entity)
        {
            _cartRepository.Update(entity);
        }

        public void RemoveFromCart(string userId,int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.RemoveFromCart(cart.Id,productId);
            }
           

        }

        public void ClearCart(int cartId)
        {
            var cart = _cartRepository.GetById(cartId);

            cart.CartItems.Clear();
            _cartRepository.Update(cart);
        }
    }
}

