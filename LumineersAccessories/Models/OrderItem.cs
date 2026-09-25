using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A single line item in an order. PricePerUnit is captured at purchase time so
    /// historical orders stay accurate even if the product price later changes.
    /// </summary>
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerUnit { get; set; }

        // Navigation properties
        [ForeignKey(nameof(OrderID))]
        public Order? Order { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product? Product { get; set; }
    }
}
