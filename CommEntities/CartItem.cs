﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Entities
{
   public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity  { get; set; }

    }
}
