using Microsoft.AspNetCore.Mvc;
using ProductManagerAPI.Data;
using ProductManagerAPI.Data.Entites;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
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

            return CreatedAtAction(
                nameof(CreateProduct),
                new { id = product.Id },
                productDto);
        }

        [HttpDelete("{sku}")]
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
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }

    }
}
