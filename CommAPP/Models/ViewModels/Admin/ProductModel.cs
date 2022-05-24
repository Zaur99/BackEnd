using Comm.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ad boş buraxıla bilməz")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Qiymət daxil edin")]
        [Range(1,10000)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Açıqlama boş buraxıla bilməz")]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Açıqlama 20-100 arası simvoldan ibarət olmalıdır")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Url boş buraxıla bilməz")]
        public string Url { get; set; }

        public string ImageUrl { get; set; }
       

        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}
