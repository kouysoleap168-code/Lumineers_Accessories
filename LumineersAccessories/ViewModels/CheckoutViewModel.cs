using System.ComponentModel.DataAnnotations;
using LumineersAccessories.Models;

namespace LumineersAccessories.ViewModels
{
    /// <summary>
    /// Backing model for the checkout page: customer info + shipping address + cart snapshot.
    /// </summary>
    public class CheckoutViewModel
    {
        // Customer info
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        // Shipping
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

        // Cart snapshot for the order summary
        public List<CartItem> CartItems { get; set; } = new();
        public decimal Subtotal => CartItems.Sum(c => c.Subtotal);
        public const decimal ShippingFee = 4.99m;
        public decimal Total => Subtotal + (CartItems.Any() ? ShippingFee : 0);
    }
}
