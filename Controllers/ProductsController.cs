using Microsoft.AspNetCore.Mvc;
using ProductManagerAPI.Data;
using ProductManagerAPI.Data.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        // Kan bara sättas i constructorn
        private readonly ApplicationDbContext context;

        // constructorn
        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IEnumerable<ProductDto> GetProducts([FromQuery] string? name)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(name)
                 ? context.Products.ToList()
                 : context.Products.Where(x => x.Name == name);

            IEnumerable<ProductDto> productDtos = products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Sku = x.Sku,
                Description = x.Description,
                Url = x.Url,
                Price = x.Price,
            });

            return productDtos;
        }

        [HttpGet("{sku}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public ActionResult<ProductDto> GetProduct(string sku)
        {
            var product = context.Products.FirstOrDefault(x => x.Sku == sku);

            if (product is null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Description = product.Description,
                Url = product.Url,
                Price = product.Price
            };

            return productDto;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        public ActionResult<ProductDto> CreateProduct(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Sku = request.Sku,
                Description = request.Description,
                Url = request.Url,
                Price = request.Price,
            };

            context.Products.Add(product);
            context.SaveChanges();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Description = product.Description,
                Url = product.Url,
                Price = product.Price,
            };

            // example localhost/8000/products/123
            return CreatedAtAction(
                nameof(CreateProduct),
                new { id = product.Id },
                productDto);
        }

        [HttpDelete("{sku}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult DeleteProduct(string sku) 
        {
            var product = context.Products.FirstOrDefault(x => x.Sku == sku);

            if (product is null) 
            {
                return NotFound();
            }

            context.Products.Remove(product);
            context.SaveChanges();

            return NoContent();
        }
    }

    public class CreateProductRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(6)]
        public string Sku { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        public int Price { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(6)]
        public string Sku { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
