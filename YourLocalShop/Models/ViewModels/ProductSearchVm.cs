namespace YourLocalShop.Models.ViewModels
{
    public class ProductSearchVm
    {
        public string? Q { get; set; }
        public string? Category { get; set; }
        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<Product> Results { get; set; } = Enumerable.Empty<Product>();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
