using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A sellable product. Each product belongs to one category.
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter a product name.")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a valid price.")]
        [Range(0.01, 100000, ErrorMessage = "Please enter a valid price.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        // Optional image URL / file name for the product photo
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        // Rating used for sorting and display (1–5)
        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        // Navigation properties
        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public Inventory? Inventory { get; set; }
    }
}
