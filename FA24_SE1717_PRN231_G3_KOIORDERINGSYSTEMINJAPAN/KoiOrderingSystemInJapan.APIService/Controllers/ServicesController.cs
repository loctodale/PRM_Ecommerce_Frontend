using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using KoiOrderingSystemInJapan.Service;
using Microsoft.AspNetCore.Mvc;
using ServiceEntity = KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.BookingRequests;
using KoiOrderingSystemInJapan.Data.Request.Services;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ServiceService _serviceSerivce;

        public ServicesController()
        {
            _serviceSerivce ??= new ServiceService();
        }

        // GET: api/Services
        [HttpGet]
        public async Task<IBusinessResult> GetServices()
        {
            return await _serviceSerivce.GetAll();
        }

        // GET: api/BookingRequests
        [HttpPost("filter")]
        public async Task<IBusinessResult> GetServices([FromBody] ServiceRequest query)
        {
            return await _serviceSerivce.GetAll(query, query.Page, query.PageSize);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetService(Guid id)
        {
            var service = await _serviceSerivce.GetById(id);

            return service;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutService(ServiceEntity.Service service)
        {

            return await _serviceSerivce.Save(service);
        }

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostService(ServiceEntity.Service service)
        {
            return await _serviceSerivce.Save(service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteService(Guid id)
        {
            return await _serviceSerivce.DeleteById(id);
        }
    }
}
