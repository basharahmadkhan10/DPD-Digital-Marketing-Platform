using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPDDigitalMarketing.Data;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Controllers.Api
{

    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? category, [FromQuery] string? search)
        {
            var query = _context.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Description.Contains(search));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || !product.IsActive)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(new { message = "ID in URL does not match body." });
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(new { message = "Product updated successfully.", product });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deactivated successfully." });
        }
    }
}
