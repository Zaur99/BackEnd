using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comm.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public int Star { get; set; }   
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        //public BaseUser User { get; set; }
    }           
}
