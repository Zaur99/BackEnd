using System;

namespace CommAPP.Models.ViewModels
{
    public class PageDetails
    {
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public string CurrentCategory { get; set; }
        public int CurrentPage { get; set; }
        //TotalItems/PageSize
        public int ShownProducts()
        {
            return (int)Math.Ceiling((decimal)TotalItems / PageSize);
        }

    }
}
