namespace LumineersAccessories.ViewModels
{
    /// <summary>Customer row with order count for the admin customers list.</summary>
    public class CustomerWithOrderCountViewModel
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int OrderCount { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
