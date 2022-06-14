using System.Collections.Generic;

namespace CommAPP.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public  CategoryViewModel Parent { get; set; }
        public  List<CategoryViewModel> Children { get; set; }
       
    }
}
