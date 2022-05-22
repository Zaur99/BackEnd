using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.Admin
{
    public class NestedCategoriesViewModel
    {
        public NestedCategoriesViewModel()
        {
            ChildItems = new List<NestedCategoriesViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

       

        public NestedCategoriesViewModel ParentId { get; set; }

        public IList<NestedCategoriesViewModel> ChildItems { get; set; }

        public void AddChildItem(NestedCategoriesViewModel childItem)
        {
            childItem.ParentId = this;
            ChildItems.Add(childItem);
        }
    }
}
