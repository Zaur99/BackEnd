using Comm.Entities;
using CommAPP.Models.ViewModels.OrderRelated;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.Admin
{
    public class OrderFullViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ExtraDetails { get; set; }
        public string Adress { get; set; }
        
        public DateTime OrderedTime { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }

       
    }

    
}
