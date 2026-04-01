using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRecordSystem.Data;
using ProductRecordSystem.Models;
using WeatherAPI.DTOs;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        return Ok(new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Phone
        });
    }

    [HttpGet("{id}/orders")]
    public async Task<ActionResult<List<OrderSummaryDto>>> GetOrdersByCustomerId(int id)
    {
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == id);
        if (!customerExists) return NotFound();

        var orders = await _context.Orders
            .Where(o => o.CustomerId == id)
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ItemCount = o.OrderItems.Count,
                TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var resultDto = new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Phone
        };

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("bulk")]
    public async Task<ActionResult> BulkInsert(IEnumerable<CreateCustomerDto> dtos)
    {
        var customers = dtos.Select(dto => new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone
        }).ToList();

        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();

        return Ok(new { count = customers.Count });
    }

    [HttpGet("with-orders")]
    public async Task<ActionResult<IEnumerable<CustomerWithOrdersDto>>> GetWithOrders()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerWithOrdersDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Orders = c.Orders.Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    ItemCount = o.OrderItems.Count,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                }).ToList()
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var count = await _context.Customers.CountAsync();
        return Ok(new { totalCustomers = count });
    }

    [HttpGet("full-details")]
    public async Task<ActionResult<IEnumerable<CustomerFullDetailsDto>>> GetFullDetails()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerFullDetailsDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Orders = c.Orders.Select(o => new OrderDetailDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    CustomerId = o.CustomerId,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDetailDto
                    {
                        OrderId = oi.OrderId,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            Name = oi.Product.Name,
                            Price = oi.Product.Price,
                            StockQty = oi.Product.StockQty,
                            CategoryId = oi.Product.CategoryId,
                            SupplierId = oi.Product.SupplierId
                        }
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();

        return Ok(customers);
    }
}
