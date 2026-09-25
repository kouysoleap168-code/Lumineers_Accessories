using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A product category (Bracelets, Necklaces, Rings, Earrings).
    /// </summary>
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please enter a category name.")]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
