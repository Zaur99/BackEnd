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
        public IEnumerable<Product> Products{ get; set; }
    }
}
