using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebAPIExercises.Data;
using WebAPIExercises.DTO;
using WebAPIExercises.Models;

namespace WebAPIExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        RetailDbContext _context;
        public StoresController(RetailDbContext retailDbContext)
        {
            _context = retailDbContext;
        }
        [HttpGet]
        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores.ToList();
        }
        [HttpGet("StoresByMonthlyIncome")]
        public List<Store> GetStoresByMonthlyIncome()
        {
            return _context.Stores.OrderByDescending(s => s.MonthlyIncome).ToList();
        }
        [HttpGet("OldestStore")]
        public Store GetTheOldestStore()
        {
            return _context.Stores.OrderByDescending(s => s.ActiveSince).ToList()[0];
        }


        [HttpGet("StoresByKeyword/{keyword}")]
        public List<Store> GetStoresByKeyword(string keyword)
        {
             var stores = _context.Stores.Where(s => s.Name.Contains(keyword) || s.City.Contains(keyword) ||
                    s.Country.Contains(keyword) || s.ActiveSince.ToString().Contains(keyword)).ToList();
            List<Store> storeDTO = new List<Store>(stores);
            return storeDTO;
        }

        [HttpGet("StoresByCountry/{country}")]
        public List<Store> GetStoresByCountry(string country)
        {
            return _context.Stores.Where(s => s.Country == country).ToList();
        }
        [HttpGet("StoresByCity/{city}")]
        public List<Store> GetStoresByCity(string city)
        {
            return _context.Stores.Where(s => s.City == city).ToList();
        }
        [HttpPost]
        public IActionResult CreateStore([FromBody]StoreDTO storeDTO)
        {
            if(storeDTO== null)
            {
                return BadRequest("Store is null");
            }

            foreach (var existingStore in _context.Stores)
            {
                if(storeDTO.Id == existingStore.Id)
                {
                    return BadRequest();
                }
            }
            Store store = new Store
            {
                Id = Guid.NewGuid(),
                Name = storeDTO.Name,
                City = storeDTO.City,
                Country = storeDTO.Country,
                OwnerName = storeDTO.OwnerName,
                MonthlyIncome = storeDTO.MonthlyIncome,
                ActiveSince = DateTime.Now,
            };

            _context.Stores.Add(store);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(Guid id)
        {
            try
            {
                var store = _context.Stores.FirstOrDefault(p => p.Id == id);
                if (store == null) 
                {
                    return BadRequest();
                }
                _context.Remove(store);
                _context.SaveChanges();
                return Ok("deleted");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{storeid}")]
        public IActionResult UpdateStore(Guid storeid, [FromBody] StoreDTO storeDTO)
        {
            try
            {
                var store = _context.Stores.FirstOrDefault(p => p.Id ==  storeid);
                if(store == null)
                {
                    return BadRequest();
                }
                store.Name = storeDTO.Name;
                store.OwnerName = storeDTO.OwnerName;
                store.City = storeDTO.City;
                store.Country = storeDTO.Country;
                store.MonthlyIncome = storeDTO.MonthlyIncome;
                _context.Stores.Update(store);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("transferOwnership/{storeId}")]
        public IActionResult TransferOwnership(Guid storeId, [FromBody] string newOwnerName)
        {
            try
            {
                var store = _context.Stores.FirstOrDefault(s => s.Id == storeId);
                if(store == null)
                {
                    return BadRequest("StoreId not found");
                }
                store.OwnerName = newOwnerName;
                _context.Stores.Update(store);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
