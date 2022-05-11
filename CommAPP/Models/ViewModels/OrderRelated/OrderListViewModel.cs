using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.OrderRelated
{
    public class OrderListViewModel
    {
        public IEnumerable<OrderViewModel> OrderListVM { get; set; }
       
    }

    public class OrderViewModel {

        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderedTime { get; set; }
        public IEnumerable<OrderItemModel> OrderItemListVM { get; set; }
    }

}
