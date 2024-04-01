using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIExercises.Data;
using WebAPIExercises.DTO;
using WebAPIExercises.Models;

namespace WebAPIExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        RetailDbContext _context;
        public InventoryController(RetailDbContext retailDbContext)
        {
            _context = retailDbContext;
        }
        [HttpGet]
        public IEnumerable<Inventory> GetInventories()
        {
            return _context.Inventories;
        }
        [HttpPost("{productId}/{storeId}")]
        public IActionResult CreateInventory(Guid productId, Guid storeId, [FromBody] InventoryDTO inventory)
        {

            if(inventory == null) { return BadRequest(); }
            var newProductId = _context.Products.FirstOrDefault(p => p.Id == productId).Id;
            if(newProductId == null) { return BadRequest("product doesn't exist"); }
            var newStoreId = _context.Stores.FirstOrDefault(s => s.Id == storeId).Id;
            if(newStoreId == null) { return BadRequest("store doesn't exist"); }
            var newInventory = new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = newProductId,
                StoreId = newStoreId,
                NrOfProducts = inventory.NrOfProducts,
            };
            _context.Inventories.Add(newInventory);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut("{inventoryId}")]
        public IActionResult UpdateInventory(Guid inventoryId, [FromBody] InventoryDTO inventory)
        {
            try
            {
                var inventoryToUpdate = _context.Inventories.FirstOrDefault(i => i.Id == inventory.Id);
                if (inventoryToUpdate == null) 
                {
                    return BadRequest();
                }
                inventoryToUpdate.NrOfProducts = inventory.NrOfProducts;
                _context.Inventories.Update(inventoryToUpdate);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{inventoryId}")]
        public IActionResult DeleteInventory(Guid inventoryId)
        {
            try
            {
                var inventoryToDelete = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);
                if(inventoryToDelete == null)
                {
                    return BadRequest();
                }
                _context.Inventories.Remove(inventoryToDelete);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        //[HttpGet("GetStoreProductsByStoreId")]
        //public IEnumerable<Product> GetStoreProductsByStoreId() 
        //{

        //}
    }
}
