using Microsoft.AspNetCore.Mvc;
using WebAPIExercises.Data;
using WebAPIExercises.DTO;
using WebAPIExercises.Models;

namespace WebAPIExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        RetailDbContext _context;
        public ProductsController(RetailDbContext retailDbContext)
        {
            _context = retailDbContext;
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        [HttpPost]
        public void CreateProduct([FromBody] ProductDTO product)
        {
            var productMap = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Ratings = product.Ratings,  
                CreatedOn = DateTime.Now
            };
            _context.Add(productMap);
            _context.SaveChanges();
        }
        [HttpDelete("{id}")]
        public void DeleteProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Remove(product);
            _context.SaveChanges();
        }   
        [HttpPut("{productid}")]
        public void UpdateProduct(Guid productid, [FromBody] ProductDTO updatedProduct)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productid);
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            _context.Update(product);
            _context.SaveChanges();
        }
        [HttpGet("GetProductById")]
        public Product GetProductById(Guid id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        [HttpGet("ProductsByKeyword/{keyword}")]
        public IEnumerable<Product> GetProductsByKeyword(string keyword)
        {
            return _context.Products.Where(p => p.Name.Contains(keyword) ||
                    p.Description.Contains(keyword) || p.CreatedOn.ToString().Contains(keyword));
        }
        [HttpGet("ProductsByAverageRatingAsc")]
        public List<Product> GetProductsByAverageRatingAsc()
        {
            return _context.Products.OrderBy(p => p.Ratings.Average()).ToList();
        }

        [HttpGet("ProductsByAverageRatingDesc")]
        public List<Product> GetProductsByAverageRatingDesc()
        {
            return _context.Products.OrderByDescending(p => p.Ratings.Average()).ToList();
        }

        [HttpGet("MostRecentProduct")]
        public Product GetMostRecentProduct()
        {
            var products = _context.Products.OrderByDescending(p => p.CreatedOn).ToList();
            return products[0];
        }
    }
}
