namespace WeatherAPI.DTOs;

public class DashboardSummaryDto
{
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalOrders { get; set; }
    public int TotalOrderItems { get; set; }
}
