namespace LumineersAccessories.ViewModels
{
    /// <summary>
    /// One line in the session-based shopping cart.
    /// </summary>
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public int StockLevel { get; set; }

        public decimal Subtotal => Price * Quantity;
    }
}
