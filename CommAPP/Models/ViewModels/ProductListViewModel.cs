using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class  PageDetails
    {
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public string CurrentCategory { get; set; }
        public int CurrentPage { get; set; }
        //TotalItems/PageSize
        public int ShownProducts()
        {
            return (int)Math.Ceiling((decimal)TotalItems / PageSize);
        }

    }
    public class ProductListViewModel
    {
        public PageDetails PageDetails { get; set; }
        public IEnumerable<ProductViewModel> Products{ get; set; }
    }

    public class ProductViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
    }
}
