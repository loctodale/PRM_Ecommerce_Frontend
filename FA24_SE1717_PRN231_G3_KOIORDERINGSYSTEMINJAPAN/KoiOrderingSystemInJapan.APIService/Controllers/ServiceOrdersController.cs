using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.ServiceOrder;
using KoiOrderingSystemInJapan.Data.Request.ServiceOrders;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderSerivce;
        private readonly IPaymentService _paymentService;
        public ServiceOrdersController()
        {
            _serviceOrderSerivce ??= new ServiceOrderService();
            _paymentService ??= new PaymentService();
        }

        // GET: api/ServiceOrders
        [HttpGet("{page}&{pageSize}")]
        public async Task<IBusinessResult> GetPagedServiceOrders(int page, int pageSize)
        {
            return await _serviceOrderSerivce.GetPagedServiceOrders(page, pageSize);
        }

        [HttpPost("filter")]
        public async Task<IBusinessResult> GetServiceOrders([FromBody]ServiceOrderRequest request)
        {
            return await _serviceOrderSerivce.GetAll(request, request.Page, request.PageSize);
        }

        // GET: api/ServiceOrders/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetServiceOrder(Guid id)
        {
            return await _serviceOrderSerivce.GetById(id);
        }

        // PUT: api/ServiceOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutServiceOrder(ServiceOrder serviceOrder)
        {
            return await _serviceOrderSerivce.Save(serviceOrder);
        }

        [HttpPut("update-isdeleted/{id}")]
        public async Task<IBusinessResult> UpdateIsDeleted(Guid id)
        {
            return await _serviceOrderSerivce.UpdateIsDeleted(id);
        }

        // POST: api/ServiceOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostServiceOrder(ServiceOrder serviceOrder)
        {
            return await _serviceOrderSerivce.Save(serviceOrder);
        }

        [HttpPost("create_payment")]
        public async Task<IBusinessResult> PostCreatePayment(RequestPaymentServiceModel request)
        {
            return await _serviceOrderSerivce.CreatePayment(request);
        }

        // DELETE: api/ServiceOrders/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteServiceOrder(Guid id)
        {
            return await _serviceOrderSerivce.DeleteById(id);
        }

    }
}
