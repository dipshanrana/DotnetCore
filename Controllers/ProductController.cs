using Microsoft.AspNetCore.Mvc;

namespace ProductApi;

// base Url : http://localhost:5033/api/Product

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> products = new List<Product>
    {
        new Product
        {
            ID = 1, Name = "Laptop", Price = 1000m,
            ImageUrl = "https://media.istockphoto.com/id/479520746/photo/laptop-with-blank-screen-on-white.jpg?s=612x612&w=0&k=20&c=V5dj0ayS8He0BP4x15WR5t5NKYzWTKv7VdWvD2SAVAM="
        },
        new Product
        {
            ID = 2, Name = "Mobile", Price = 200m,
            ImageUrl = "https://media.istockphoto.com/id/479520746/photo/laptop-with-blank-screen-on-white.jpg?s=612x612&w=0&k=20&c=V5dj0ayS8He0BP4x15WR5t5NKYzWTKv7VdWvD2SAVAM="
        }
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
        var existingProduct = products.FirstOrDefault(p => p.ID == id);
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
        newProduct.ID = products.Max(p => p.ID) + 1; // make id unique each time
        products.Add(newProduct);
        return Ok("Success");
    }


    //Delete
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existingProduct = products.FirstOrDefault(p => p.ID == id);
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
        var existingProduct = products.FirstOrDefault(p => p.ID == id);
        if (existingProduct == null)
        {
            return NotFound(); // 404 error

        }
        products.Remove(existingProduct);
        product.ID = id;
        products.Add(product);
        return Ok(product);
    }
}
