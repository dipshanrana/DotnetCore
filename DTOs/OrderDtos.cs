using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.DTOs;

public class CreateOrderDto
{
    [Required]
    public int CustomerId { get; set; }

    public string Status { get; set; } = "Pending";

    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderDto
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CustomerId { get; set; }
}

public class OrderSummaryDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderDetailDto : OrderDto
{
    public CustomerDto Customer { get; set; } = new();
    public List<OrderItemDetailDto> OrderItems { get; set; } = new();
}
