using Microsoft.AspNetCore.Mvc;
using ProductRecordSystem.Models;

namespace ProductApi;

// base Url : http://localhost:5033/api/Product

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> products = new List<Product>
    {
    };

    //  Url : http://localhost:5033/api/Product/getAll
    [HttpGet()]
    [Route("getAll")]
    public ActionResult<List<Product>> Get()
    {
        return Ok(products);
    }

    //  Url : http://localhost:5033/api/Product/{id}
    [HttpGet("{id}")]
    public ActionResult<Product> Get(int id)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            return NotFound(); // 404 error

        }
        return Ok(existingProduct);
    }


    //Post operation // ADD operation
    [HttpPost]
    public ActionResult<Product> Post(Product newProduct)
    {
        newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1; // make id unique each time
        products.Add(newProduct);
        return Ok("Success");
    }


    //Delete
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            return NotFound(); // 404 error

        }
        products.Remove(existingProduct);
        return NoContent();
    }


    //Update
    [HttpPut("{id}")]
    public ActionResult Put(int id, Product product)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            return NotFound(); // 404 error

        }
        products.Remove(existingProduct);
        product.Id = id;
        products.Add(product);
        return Ok(product);
    }
}
