using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.OrderRelated
{
    public class OrderModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string City { get; set; }

        public string ExtraDetails { get; set; }
        [Required]

        public string Adress { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemModel> OrderItemsModel { get; set; }
        public List<CartItemModel> CartItemModels { get; set; }

    }
}
