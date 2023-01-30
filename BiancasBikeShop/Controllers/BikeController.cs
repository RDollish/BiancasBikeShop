using BiancasBikeShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiancasBikeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private IBikeRepository _bikeRepo;

        public BikeController(IBikeRepository bikeRepo)
        {
            _bikeRepo = bikeRepo;
        }

        [Route("")]
        public IActionResult Get()
        {
            var bikes = _bikeRepo.GetAllBikes();
            return Ok(bikes);
        }

        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            var bike = _bikeRepo.GetBikeById(id);
            if (bike == null)
            {
                return NotFound();
            }
            return Ok(bike);
        }

        [Route("GetBikesInShopCount")]
        public IActionResult GetBikesInShopCount()
        {
            var count = _bikeRepo.GetBikesInShopCount();
            return Ok(count);
        }
    }

}
