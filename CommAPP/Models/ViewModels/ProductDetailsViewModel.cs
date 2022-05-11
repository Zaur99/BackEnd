using Comm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
       
        //VM for filling by user in order to post comment
        public CommentViewModel CommentVM { get; set; }

        //All comments which belong to Product
        public IEnumerable<CommentListViewModel> CommentListVM { get; set; }

    }
}
