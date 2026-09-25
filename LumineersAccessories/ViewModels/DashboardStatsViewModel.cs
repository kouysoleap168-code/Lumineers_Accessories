namespace LumineersAccessories.ViewModels
{
    /// <summary>
    /// Statistics shown on the admin dashboard.
    /// </summary>
    public class DashboardStatsViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockCount { get; set; }
        public List<LowStockProductViewModel> LowStockProducts { get; set; } = new();
        public List<SalesPointViewModel> SalesByDay { get; set; } = new();
    }

    public class LowStockProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int StockLevel { get; set; }
        public string StockStatus { get; set; } = string.Empty;
    }

    public class SalesPointViewModel
    {
        public string Label { get; set; } = string.Empty; // e.g. date
        public decimal Total { get; set; }
    }
}
