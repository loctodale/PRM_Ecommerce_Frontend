using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryDetailsController : ControllerBase
    {

        private readonly DeliveryDetailService _deliverydetailSerivce;

        public DeliveryDetailsController()
        {
            _deliverydetailSerivce ??= new DeliveryDetailService();
        }

        // GET: api/DeliveryDetails
        [HttpGet]
        public async Task<IBusinessResult> GetDeliveryDetails()
        {
            return await _deliverydetailSerivce.GetAll();
        }

        // GET: api/DeliveryDetails/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetDeliveryDetail(Guid id)
        {
            return await _deliverydetailSerivce.GetById(id);
        }

        // GET: api/DeliveryDetails/5
        [HttpGet("all/{id}")]
        public async Task<IBusinessResult> GetAllDelieryDetailById(Guid id)
        {
            return await _deliverydetailSerivce.GetAllById(id);
        }

        // PUT: api/DeliveryDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutDeliveryDetail(DeliveryDetail deliveryDetail)
        {

            return await _deliverydetailSerivce.Save(deliveryDetail);
        }

        // POST: api/DeliveryDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostDeliveryDetail(DeliveryDetail deliveryDetail)
        {
            return await _deliverydetailSerivce.Save(deliveryDetail);
        }

        // DELETE: api/DeliveryDetails/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteDeliveryDetail(Guid id)
        {
            return await _deliverydetailSerivce.DeleteById(id);
        }

        [HttpGet("search")]
        public async Task<IBusinessResult> SearchDeliveryDeatil([FromQuery] string? deliveryname, [FromQuery] bool? isdeleted, [FromQuery] string? description, [FromQuery] int page, [FromQuery] int  pagesize)
        {
            return await _deliverydetailSerivce.SearchDeliveryDetail(deliveryname, isdeleted, description , page , pagesize);
        }

    }
}
