using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class CategoryListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Category Category { get; set; }
        public string SelectedCategory { get; set; }
        //public List<Product> Products { get; set; }
    }
}
