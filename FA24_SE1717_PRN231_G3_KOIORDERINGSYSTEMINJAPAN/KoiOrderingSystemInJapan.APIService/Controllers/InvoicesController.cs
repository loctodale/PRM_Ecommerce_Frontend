using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController() => _invoiceService ??= new InvoiceService();

        [HttpGet]
        public async Task<IBusinessResult> GetInvoices([FromQuery] decimal? paymentAmount, [FromQuery] bool? isDeleted, [FromQuery] string? note, [FromQuery] int page, [FromQuery] int pageSize)
        {
            return await _invoiceService.GetAll(paymentAmount, isDeleted, note, page, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetInvoice(Guid id)
        {
            return await _invoiceService.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutInvoice(Invoice invoice)
        {
            return await _invoiceService.Save(invoice);
        }

        [HttpPost]
        public async Task<IBusinessResult> PostInvoice(Invoice invoice)
        {
            return await _invoiceService.Save(invoice);
        }

        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteInvoice(Guid id)
        {
            return await _invoiceService.DeleteById(id);
        }

    }
}
