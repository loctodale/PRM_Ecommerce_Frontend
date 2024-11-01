using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>
    {
        public InvoiceRepository() { }

        public InvoiceRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        
        public  async Task<(List<Invoice> Items, int TotalPages)> GetAll(decimal? paymentAmount, bool? isDeleted, string? note, int page, int pageSize)
        {
             var invoices = await _context.Invoices.ToListAsync();
            if(paymentAmount.HasValue)
            {
                invoices = invoices.Where(x => x.PaymentAmount == paymentAmount).ToList();
            }
            if (isDeleted.HasValue)
            {
                invoices = invoices.Where(x => x.IsDeleted == isDeleted).ToList();
            }
            if (note != null)
            {
                invoices = invoices.Where(x => x.Note == note).ToList();
            }

            var totalItems = invoices.Count();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = invoices.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (items, totalPage);
        }
        public Invoice GetByIdNoTracking(Guid code)
        {
            //return _context.Set<T>().AsNoTracking().Where(e => EF.Property<Guid>(e, "Id") == code).FirstOrDefault();
            return _context.Invoices.AsNoTracking().FirstOrDefault(x => x.Id == code);

        }
    }
}
