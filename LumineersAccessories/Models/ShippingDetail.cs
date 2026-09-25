using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// Shipping information for an order. One order has exactly one shipping detail record.
    /// </summary>
    public class ShippingDetail
    {
        [Key]
        public int ShippingID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please enter your address.")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your city.")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your postal code.")]
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Tracking Number")]
        public string? TrackingNumber { get; set; }

        // Navigation property
        [ForeignKey(nameof(OrderID))]
        public Order? Order { get; set; }
    }
}
