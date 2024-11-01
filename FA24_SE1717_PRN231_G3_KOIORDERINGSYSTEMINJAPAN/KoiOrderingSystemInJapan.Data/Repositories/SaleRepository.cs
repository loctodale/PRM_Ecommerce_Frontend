using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.BookingRequests;
using KoiOrderingSystemInJapan.Data.Request.Sale;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class SaleRepository : GenericRepository<Sale>
    {
        public SaleRepository() { }
        public SaleRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales.Include(e => e.BookingRequest).Include(e => e.SaleStaff).ToListAsync();
        }

        public async Task<(List<Sale>, int)> GetAllAsync(SaleRequest query, int page, int pageSize)
        {
            var queryable = _context.Set<Sale>().AsQueryable();

            if (!query.ProposalDetails.IsNullOrEmpty())
            {
                queryable = queryable.Where(m => m.ProposalDetails.Trim().Contains(query.ProposalDetails.Trim()));
            }

            if (query.TotalPrice.HasValue)
            {
                queryable = queryable.Where(m => m.TotalPrice == query.TotalPrice);
            }

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(m => m.Status == query.Status);
            }

            if (query.ResponseDate.HasValue)
            {
                queryable = queryable.Where(m => m.ResponseDate == query.ResponseDate);
            }

            if (!query.ResponseBy.IsNullOrEmpty())
            {
                queryable = queryable.Where(m => m.ResponseBy.Trim().Contains(query.ResponseBy.Trim()));
            }


            if (query.CreatedDate.HasValue)
            {
                var createdDate = query.CreatedDate.Value.Date;
                queryable = queryable.Where(m =>
                    m.CreatedDate.HasValue &&
                    m.CreatedDate.Value.Year == createdDate.Year &&
                    m.CreatedDate.Value.Month == createdDate.Month &&
                    m.CreatedDate.Value.Day == createdDate.Day);
            }

            if (query.UpdatedDate.HasValue)
            {
                var updatedDate = query.UpdatedDate.Value.Date;
                queryable = queryable.Where(m =>
                    m.UpdatedDate.HasValue &&
                    m.UpdatedDate.Value.Year == updatedDate.Year &&
                    m.UpdatedDate.Value.Month == updatedDate.Month &&
                    m.UpdatedDate.Value.Day == updatedDate.Day);
            }

            var totalItems = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            queryable = queryable.Where(m => m.IsDeleted==false).Include(m => m.BookingRequest).Include(m => m.SaleStaff);
            var data = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalPages);
        }

        public async Task<Sale> GetByIdIncludeAsync(Guid id)
        {
            return await _context.Sales.Where(e => e.Id == id).Include(e => e.BookingRequest)
                                                                .ThenInclude(br => br.Customer)
                                                            .Include(e => e.BookingRequest)
                                                                .ThenInclude(br => br.Travel)
                                                            .Include(e => e.SaleStaff).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateIsDeleted(Sale sale)
        {
            sale.IsDeleted = true;
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                return true;
            }
            return false;
        }
    }
}
