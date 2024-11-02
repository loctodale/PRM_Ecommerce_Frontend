using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {

        private readonly DeliveryService _deliverySerivce;

        public DeliveriesController()
        {
            _deliverySerivce ??= new DeliveryService();
        }

        // GET: api/Deliveries
        [HttpGet]
        public async Task<IBusinessResult> GetDeliveries()
        {
            return await _deliverySerivce.GetAll();
        }

        // GET: api/Deliveries/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetDelivery(Guid id)
        {
            var delivery = await _deliverySerivce.GetById(id);

            return delivery;
        }

        // PUT: api/Deliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutDelivery(Delivery delivery)
        {

            return await _deliverySerivce.Save(delivery);
        }

        // POST: api/Deliveries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostDelivery(Delivery delivery)
        {
            return await _deliverySerivce.Save(delivery);
        }

        // DELETE: api/Deliveries/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteDelivery(Guid id)
        {
            return await _deliverySerivce.DeleteById(id);
        }

        [HttpGet("search")]
        public async Task<IBusinessResult> SearchDelivery([FromQuery] string? deliveryname, [FromQuery] string? code, [FromQuery] string? location, [FromQuery] int page, [FromQuery] int pageSize)
        {
            return await _deliverySerivce.SearchDelivery(deliveryname, code, location , page, pageSize);
        }

    }
}
