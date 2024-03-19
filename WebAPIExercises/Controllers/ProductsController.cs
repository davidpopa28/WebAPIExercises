using Microsoft.AspNetCore.Mvc;
using WebAPIExercises.Models;

namespace WebAPIExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public static List<Product> _products = new()
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mouse Hama",
                Description = "Cool Mouse B)",
                Ratings = [2,4,1,2]
            },

            new Product
            {
                Id = Guid.NewGuid(),
                Name = "HP Laptop",
                Description = "Stole this laptop from work, please buy it.",
                Ratings = [1,1,1,1]
            },

            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Watch",
                Description = "Citizen Watch, 250$",
                Ratings = [4,4,4,4]
            },
        };

        [HttpGet]
        public List<Product> GetAllProducts()
        {
            return _products;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Store is null");
            }

            foreach (var existingProduct in _products)
            {
                if (product.Id == existingProduct.Id)
                {
                    return BadRequest();
                }
            }

            _products.Add(product);
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            foreach (var existingProduct in _products)
            {
                if (existingProduct.Id == id)
                {
                    _products.Remove(existingProduct);
                    return Ok();
                }
            }
            return NotFound();
        }
        [HttpPut("transfer-ownership/{productid}")]
        public IActionResult UpdateStore(Guid productid, [FromBody] Product product)
        {
            for(int i=0;i<_products.Count;i++)
            {
                if (_products[i].Id == productid)
                {
                    _products[i] = product;
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpGet("ProductsByKeyword/{keyword}")]
        public List<Product> GetProductsByKeyword(string keyword)
        {
            List<Product> products = new();
            foreach (var existingProduct in _products)
            {
                int rating = 0;
                if (existingProduct.Name.Contains(keyword))
                {
                    products.Add(existingProduct);
                }
                else if (existingProduct.Description.Contains(keyword))
                {
                    products.Add(existingProduct);
                }
                else if (int.TryParse(keyword, out rating) && existingProduct.Ratings.Contains(int.Parse(keyword)))
                {
                    products.Add(existingProduct);
                }
            }
            return products;
        }
        [HttpGet("ProductsByAverageRatingAsc")]
        public List<Product> GetProductsByAverageRatingAsc()
        {
            //cu linq
            //return _products.OrderByDescending(p => p.Ratings.Average()).ToList();
            List<Product> products = new List<Product>(_products);
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = 0; j < products.Count - i - 1; j++)
                {
                    if (products[j].Ratings.Average() > products[j + 1].Ratings.Average())
                    {
                        (products[j], products[j + 1]) = (products[j + 1], products[j]);
                    }
                }
            }
            return products;
        }

        [HttpGet("ProductsByAverageRatingDesc")]
        public List<Product> GetProductsByAverageRatingDesc()
        {
            //cu linq
            //return _products.OrderByDescending(p => p.Ratings.Average()).ToList();
            List<Product> products = new List<Product>(_products);
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = 0; j < products.Count - i - 1; j++)
                {
                    if (products[j].Ratings.Average() < products[j + 1].Ratings.Average())
                    {
                        (products[j], products[j + 1]) = (products[j + 1], products[j]);
                    }
                }
            }
            return products;
        }

        [HttpGet("MostRecentProduct")]
        public Product GetMostRecentProduct()
        {
            Product product = _products[0];
            foreach (var existingProduct in _products)
            {
                if(existingProduct.CreatedOn > product.CreatedOn)
                {
                    product = existingProduct;
                }
            }
            return product;
        }

        [HttpGet("OldestProduct")]
        public Product GetOldestProduct()
        {
            Product product = _products[0];
            foreach (var existingProduct in _products)
            {
                if (existingProduct.CreatedOn < product.CreatedOn)
                {
                    product = existingProduct;
                }
            }
            return product;
        }
    }
}
