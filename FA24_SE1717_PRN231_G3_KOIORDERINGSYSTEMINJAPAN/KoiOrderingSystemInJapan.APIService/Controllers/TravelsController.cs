using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using KoiOrderingSystemInJapan.Service;
using Microsoft.AspNetCore.Mvc;
using KoiOrderingSystemInJapan.Data.Request.Travels;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : ControllerBase
    {

        private readonly TravelService _travelSerivce;

        public TravelsController()
        {
            _travelSerivce ??= new TravelService();
        }

        // GET: api/Travels
        [HttpGet]
        public async Task<IBusinessResult> GetTravels()
        {
            return await _travelSerivce.GetAll();
        }
        // GET: api/Travels?key=????
        [HttpGet("search")]
        public async Task<IBusinessResult> GetTravelBySearchKey([FromQuery] string key)
        {
            return await _travelSerivce.GetTravelBySearchKeyAsync(key);
        }

        // GET: api/Travels/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetTravel(Guid id)
        {
            var travel = await _travelSerivce.GetById(id);

            return travel;
        }

        [HttpPost("filter")]
        public async Task<IBusinessResult> GetTravels([FromBody] TravelRequest query)
        {
            return await _travelSerivce.GetAll(query, query.Page, query.PageSize);
        }

        // PUT: api/Travels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutTravel(Travel travel)
        {

            return await _travelSerivce.Save(travel);
        }

        // POST: api/Travels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostTravel(Travel travel)
        {
            return await _travelSerivce.Save(travel);
        }

        // DELETE: api/Travels/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteTravel(Guid id)
        {
            return await _travelSerivce.DeleteById(id);
        }

    }
}
