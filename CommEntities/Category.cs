using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using System.Text;

namespace Comm.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual List<Category> Children { get; set; }

        public virtual List<ProductCategory> ProductCategories { get; set; }


    }
}
