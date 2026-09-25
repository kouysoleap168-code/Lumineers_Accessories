using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A customer order. TotalAmount is the sum of all order items at the time of purchase.
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerID { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Navigation properties
        [ForeignKey(nameof(CustomerID))]
        public Customer? Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ShippingDetail? ShippingDetail { get; set; }
    }
}
