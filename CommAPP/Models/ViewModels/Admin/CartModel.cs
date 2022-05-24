using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class CartModel
    {
        public CartModel()
        {
            CartItems = new List<CartItemModel>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public List<CartItemModel> CartItems{ get; set; }
        
        public decimal TotalPrice { 
            get { return CartItems.Sum(i => i.Price * i.Quantity); }
           
        }
       
    }

    public class CartItemModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Star { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int Quantity { get; set; }
    }
}
