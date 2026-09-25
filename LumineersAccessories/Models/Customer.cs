using System.ComponentModel.DataAnnotations;

namespace LumineersAccessories.Models
{
    /// <summary>
    /// A registered customer of the store. Passwords are stored as BCrypt hashes.
    /// </summary>
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [StringLength(30)]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
