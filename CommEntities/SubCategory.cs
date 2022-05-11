using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }  
    }
}
