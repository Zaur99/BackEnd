using Comm.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels
{
    public class ProductListViewModel
    {
        public PageDetails PageDetails { get; set; }
        public IEnumerable<ProductViewModel> Products{ get; set; }
    }
}
