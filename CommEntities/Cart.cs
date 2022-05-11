using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Entities
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
       
        
        public virtual List<CartItem> CartItems { get; set; }
    }
}
