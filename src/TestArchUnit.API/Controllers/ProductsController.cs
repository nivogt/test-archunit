using Microsoft.AspNetCore.Mvc;

namespace TestArchUnit.API.Controllers;

/// <summary>
/// Sample Products controller demonstrating a simple REST API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m },
        new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
    };

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(Products);
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public ActionResult<Product> Create([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
            return BadRequest("Invalid product data");

        var newProduct = new Product
        {
            Id = Products.Max(p => p.Id) + 1,
            Name = request.Name,
            Price = request.Price
        };

        Products.Add(newProduct);
        return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateProductRequest request)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.Name = request.Name;

        if (request.Price.HasValue && request.Price > 0)
            product.Price = request.Price.Value;

        return NoContent();
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        Products.Remove(product);
        return NoContent();
    }
}

/// <summary>
/// Product model
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Request model for creating a product
/// </summary>
public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Request model for updating a product
/// </summary>
public class UpdateProductRequest
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}
