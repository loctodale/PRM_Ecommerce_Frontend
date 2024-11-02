using KoiOrderingSystemInJapan.Data.Migrations;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.KoiOrders;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiOrdersController : ControllerBase
    {
        private readonly IKoiOrderService _koiOrderService;

        public KoiOrdersController() => _koiOrderService ??= new KoiOrderService();

        [HttpGet]
        public async Task<IBusinessResult> GetInvoices()
        {
            return await _koiOrderService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetInvoice(Guid id)
        {
            return await _koiOrderService.GetById(id);
        }

        [HttpGet("search")]
        public async Task<IBusinessResult> SearchKoiOrder([FromQuery]string? customerName, [FromQuery] decimal? price, [FromQuery] int? quantity, [FromQuery]int page, [FromQuery]int pageSize)
        {
             var result = await _koiOrderService.SearchKoiOrder(customerName, price, quantity, page, pageSize);
            return result;
        }

        [HttpGet("get_with_detail/{id}")]
        public async Task<IBusinessResult> GetByIdWithOrderDetail(Guid id)
        {
            return await _koiOrderService.GetByIdWithOrderDetail(id);
        }

        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutInvoice(KoiOrder koiOrder)
        {
            return await _koiOrderService.Save(koiOrder);

        }

        [HttpPost]
        public async Task<IBusinessResult> PostInvoice(KoiOrder koiOrder)
        {
            return await _koiOrderService.Save(koiOrder);
        }
        [HttpPost("create_payment")]
        public async Task<IBusinessResult> CreatePayment(RequestPaymentKoiOrderModel request)
        {
            return await _koiOrderService.CreatePayment(request);
        }
        [HttpPost("update_koiOrder")]
        public async Task<IBusinessResult> UpdateKoiOrder(RequestUpdateKoiOrderModel model)
        {
            return await _koiOrderService.UpdateOrder(model);
        }
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteInvoice(Guid id)
        {
            return await _koiOrderService.DeleteById(id);
        }

    }
}
