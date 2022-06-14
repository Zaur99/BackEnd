namespace CommAPP.Models.ViewModels
{
    public class ProductViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
    }
}
