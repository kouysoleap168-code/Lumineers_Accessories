using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// Stock record for a product. One product has exactly one inventory record.
    /// </summary>
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int StockLevel { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey(nameof(ProductID))]
        public Product? Product { get; set; }

        [NotMapped]
        public string StockStatus => StockLevel switch
        {
            0 => "Out of Stock",
            <= 5 => "Low Stock",
            _ => "In Stock"
        };
    }
}
