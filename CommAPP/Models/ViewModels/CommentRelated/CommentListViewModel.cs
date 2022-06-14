using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class CommentListViewModel
    {
        public string UserName { get; set; }
        public int Star { get; set; }
        public string Text { get; set; }
    }
}
