using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRecordSystem.Data;
using ProductRecordSystem.Models;
using WeatherAPI.DTOs;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll()
    {
        var suppliers = await _context.Suppliers
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone
            })
            .ToListAsync();

        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetById(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        return Ok(new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            Phone = supplier.Phone
        });
    }

    [HttpGet("{id}/products")]
    public async Task<ActionResult<List<ProductSummaryDto>>> GetSupplierProducts(int id)
    {
        var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == id);
        if (!supplierExists) return NotFound();

        var products = await _context.Products
            .Where(p => p.SupplierId == id)
            .Select(p => new ProductSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQty = p.StockQty
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        var resultDto = new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            Phone = supplier.Phone
        };

        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSupplierDto dto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        supplier.Name = dto.Name;
        supplier.Email = dto.Email;
        supplier.Phone = dto.Phone;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        // Check if supplier has products before deleting if we want to be safe, 
        // but the DB configuration says DeleteBehavior.Restrict for products.
        var hasProducts = await _context.Products.AnyAsync(p => p.SupplierId == id);
        if (hasProducts)
        {
            return BadRequest("Cannot delete supplier as it has associated products.");
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("with-products")]
    public async Task<ActionResult<IEnumerable<SupplierWithProductsDto>>> GetWithProducts()
    {
        var suppliers = await _context.Suppliers
            .Select(s => new SupplierWithProductsDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                Products = s.Products.Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQty = p.StockQty
                }).ToList()
            })
            .ToListAsync();

        return Ok(suppliers);
    }

    [HttpGet("product-counts")]
    public async Task<ActionResult<IEnumerable<SupplierProductCountDto>>> GetProductCounts()
    {
        var counts = await _context.Suppliers
            .Select(s => new SupplierProductCountDto
            {
                SupplierId = s.Id,
                SupplierName = s.Name,
                ProductCount = s.Products.Count
            })
            .ToListAsync();

        return Ok(counts);
    }
}
