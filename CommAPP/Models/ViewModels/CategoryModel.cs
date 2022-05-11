using Comm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ad boş buraxıla bilməz")]

        
        public string Name { get; set; }

        [Required(ErrorMessage = "Url boş buraxıla bilməz")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}
