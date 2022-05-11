using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.OrderRelated
{
    public class OrderItemModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
