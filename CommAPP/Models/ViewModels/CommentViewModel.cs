using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class CommentViewModel
    {
        
        [Required]
        public int Star { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
