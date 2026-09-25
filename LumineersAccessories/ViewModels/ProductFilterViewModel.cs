using LumineersAccessories.Models;

namespace LumineersAccessories.ViewModels
{
    /// <summary>
    /// Filtering / sorting / paging model for the product listing page.
    /// </summary>
    public class ProductFilterViewModel
    {
        public string? Search { get; set; }
        public int? CategoryID { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Sort { get; set; } = "newest";

        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
