using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebAPIExercises.Models;

namespace WebAPIExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        public static readonly List<Store> _stores = new()
        {
            new Store
            {
                Id=Guid.NewGuid(),
                Name="gucci",
                City="micy",
                Country = "Bro",
                MonthlyIncome= 2,
                OwnerName= "OwnerName1",
                ActiveSince= DateTime.UtcNow
            },

            new Store
            {
                Id=Guid.NewGuid(),
                Name="mucci",
                City="vicy",
                Country = "Chad",
                MonthlyIncome= 3,
                OwnerName= "OwnerName2",
                ActiveSince= DateTime.UtcNow
            }
        };

        [HttpGet]
        public List<Store> GetAllStores()
        {
            return _stores;
        }
        [HttpGet("StoresByMonthlyIncome")]
        public List<Store> GetStoresByMonthlyIncome()
        {
            //cu linq
            //return _stores.OrderByDescending(s => s.MonthlyIncome).ToList();
            List<Store> stores = new List<Store>(_stores);
            for (int i = 0; i < stores.Count - 1; i++)
            {
                for (int j = 0; j < stores.Count - i - 1; j++)
                {
                    if (stores[j].MonthlyIncome < stores[j + 1].MonthlyIncome)
                    {
                        (stores[j], stores[j + 1]) = (stores[j + 1], stores[j]);
                    }
                }
            }
            return stores;
        }
        [HttpGet("OldestStore")]
        public Store GetTheOldestStore()
        {
            //cu linq
            //return _stores.OrderByDescending(s => s.ActiveSince).ToList()[0];
            List<Store> stores = new List<Store>(_stores);
            for (int i = 0; i < stores.Count - 1; i++)
            {
                for (int j = 0; j < stores.Count - i - 1; j++)
                {
                    if (stores[j].ActiveSince > stores[j + 1].ActiveSince)
                    {
                        (stores[j], stores[j + 1]) = (stores[j + 1], stores[j]);
                    }
                }
            }
            return stores[stores.Count - 1];

        }


        [HttpGet("StoresByKeyword/{keyword}")]
        public List<Store> GetStoresByKeyword(string keyword)
        {
            List<Store> stores = new();
            foreach (Store store in _stores)
            {
                if(store.OwnerName.Contains(keyword))
                {
                    stores.Add(store);
                }
                else if(store.Name.Contains(keyword))
                {
                    stores.Add(store);
                }
            }
            return stores;
        }

        [HttpGet("StoresByCountry/{country}")]
        public List<Store> GetStoresByCountry(string country)
        {
            List<Store> stores = new();
            foreach (Store store in _stores)
            {
                if (store.Country == country)
                {
                    stores.Add(store);
                }
            }
            return stores;
        }
        [HttpGet("StoresByCity/{city}")]
        public List<Store> GetStoresByCity(string city)
        {
            List<Store> stores = new();
            foreach (Store store in _stores)
            {
                if (store.City == city)
                {
                    stores.Add(store);
                }
            }
            return stores;
        }
        [HttpPost]
        public IActionResult CreateStore([FromBody]Store store)
        {
            if(store== null)
            {
                return BadRequest("Store is null");
            }

            foreach (var existingStore in _stores)
            {
                if(store.Id == existingStore.Id)
                {
                    return BadRequest();
                }
            }

            _stores.Add(store);
            return Ok(store);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(Guid id)
        {
            foreach (var existingStore in _stores)
            {
                if(existingStore.Id== id)
                {
                    _stores.Remove(existingStore);
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPut("transfer-ownership/{storeid}")]
        public IActionResult UpdateStore(Guid storeid, [FromBody] string name)
        {
            foreach (var existingStore in _stores)
            {
                if (existingStore.Id == storeid)
                {
                    existingStore.OwnerName = name;
                    return Ok();
                }
            }
            return NotFound();
        }
    }
}
