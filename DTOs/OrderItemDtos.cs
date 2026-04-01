using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.DTOs;

public class CreateOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal UnitPrice { get; set; }
}

public class AddOrderItemToExistingDto : CreateOrderItemDto
{
    [Required]
    public int OrderId { get; set; }
}

public class OrderItemDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderItemDetailDto : OrderItemDto
{
    public ProductDto Product { get; set; } = new();
}

public class OrderItemFullDetailsDto : OrderItemDetailDto
{
    public OrderDto Order { get; set; } = new();
}

public class RemoveOrderItemDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }
}
