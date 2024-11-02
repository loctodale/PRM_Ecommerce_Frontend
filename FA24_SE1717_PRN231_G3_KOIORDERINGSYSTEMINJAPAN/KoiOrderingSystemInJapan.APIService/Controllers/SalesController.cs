using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Sale;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private ISaleService saleService;

        public SalesController()
        {
            saleService ??= new SaleService();
        }

        // GET: api/Sales
        [HttpPost("filter")]
        public async Task<IBusinessResult> GetSales([FromBody]SaleRequest request)
        {
            return await saleService.GetAll(request, request.Page, request.PageSize);
        }

        // GET: api/Sales/5
        [HttpGet("get-by-id/{id}")]
        public async Task<IBusinessResult> GetSale(Guid id)
        {
            return await saleService.GetById(id);

        }

        // GET: api/Sales/
        //[HttpGet("{status}")]
        //public async Task<IBusinessResult> GetSaleByStatus(String Status)
        //{
        //    return await saleService.GetById(id);

        //}

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IBusinessResult> PutSale(Sale sale)
        {
            return await saleService.Save(sale);
        }

        [HttpPut("update-isdeleted/{id}")]
        public async Task<IBusinessResult> PutSaleDelete(Guid id)
        {
            return await saleService.UpdateIsDeleted(id);
        }

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostSale(Sale sale)
        {
            return await saleService.Save(sale);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteSale(Guid id)
        {
            return await saleService.DeleteById(id);
        }

    }
}
