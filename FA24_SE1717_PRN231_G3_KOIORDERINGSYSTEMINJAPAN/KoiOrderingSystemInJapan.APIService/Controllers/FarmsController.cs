using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using KoiOrderingSystemInJapan.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {

        private readonly FarmService _farmSerivce;

        public FarmsController()
        {
            _farmSerivce ??= new FarmService();
        }

        // GET: api/Farms
        [HttpGet]
        public async Task<IBusinessResult> GetFarms()
        {
            return await _farmSerivce.GetAll();
        }

        // GET: api/Farms/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetFarm(Guid id)
        {
            var farm = await _farmSerivce.GetById(id);

            return farm;
        }

        // PUT: api/Farms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutFarm(Farm farm)
        {

            return await _farmSerivce.Save(farm);
        }

        // POST: api/Farms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostFarm(Farm farm)
        {
            return await _farmSerivce.Save(farm);
        }

        // DELETE: api/Farms/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteFarm(Guid id)
        {
            return await _farmSerivce.DeleteById(id);
        }

    }
}
