using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A product saved by a customer for later. A customer can wishlist a given product only once.
    /// </summary>
    public class Wishlist
    {
        [Key]
        public int WishlistID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(CustomerID))]
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product? Product { get; set; }
    }
}
