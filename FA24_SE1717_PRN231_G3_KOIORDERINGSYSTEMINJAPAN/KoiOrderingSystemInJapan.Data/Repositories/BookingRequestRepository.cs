using Azure;
using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.BookingRequests;
using Microsoft.EntityFrameworkCore;


namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class BookingRequestRepository : GenericRepository<BookingRequest>
    {
        public BookingRequestRepository() { }

        public BookingRequestRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<BookingRequest>> GetAllAsync()
        {
            return await _context.Set<BookingRequest>().AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Include(m => m.Customer)
                .Include(m => m.Travel)
                .ToListAsync();
        }

        public async Task<(List<BookingRequest>, int)> GetAllAsync(BookingRequestRequest query, int page, int pageSize)
        {
            var queryable =_context.Set<BookingRequest>().AsQueryable()
                .Include(m => m.Customer).Include(m => m.Travel).AsNoTracking();

            if (!string.IsNullOrEmpty(query.Travel?.Name))
            {
                queryable = queryable.Where(m => m.Travel.Name.Trim().ToLower().Contains(query.Travel.Name.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Customer?.Email))
            {
                queryable = queryable.Where(m => m.Customer.Email.Trim().ToLower().Contains(query.Customer.Email.Trim().ToLower()));
            }

            if (query.QuantityService.HasValue)
            {
                queryable = queryable.Where(m => m.QuantityService == query.QuantityService);
            }

            if (query.NumberOfPerson.HasValue)
            {
                queryable = queryable.Where(m => m.NumberOfPerson == query.NumberOfPerson);
            }

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(m => m.Status == query.Status);
            }

            // Base 
            if (!string.IsNullOrEmpty(query.CreatedBy))
            {
                queryable = queryable.Where(m => m.CreatedBy.Trim().ToLower().Contains(query.CreatedBy.Trim().ToLower()));
            }


            if (query.CreatedDate.HasValue)
            {
                queryable = queryable.Where(m => m.CreatedDate == query.CreatedDate);
            }

            if (!string.IsNullOrEmpty(query.UpdatedBy))
            {
                queryable = queryable.Where(m => m.UpdatedBy.Trim().ToLower().Contains(query.UpdatedBy.Trim().ToLower()));
            }

            if (query.UpdatedDate.HasValue)
            {
                queryable = queryable.Where(m => m.UpdatedDate == query.UpdatedDate);
            }

            if (!string.IsNullOrEmpty(query.Note))
            {
                queryable = queryable.Where(m => m.Note.Trim().ToLower().Contains(query.Note.Trim().ToLower()));
            }

            var totalItems = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var data = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalPages);
        }

        public async Task<BookingRequest> GetByIdAsync(Guid id)
        {
            return await _context.Set<BookingRequest>().AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Include(m => m.Customer) 
                .Include(m => m.Travel)  
                .SingleOrDefaultAsync(b => b.Id == id); 
        }

        public async Task<List<BookingRequest>> GetBookingRequestsWithNoSale()
        {
            return await _context.Set<BookingRequest>().AsNoTracking()
                .Where(m => !m.IsDeleted && m.Sale == null)
                .Include(m => m.Customer)
                .Include(m => m.Travel).ToListAsync();
        }


        public async Task<List<BookingRequest>> GetBookingRequestsWithNoFilter()
        {
            return await _context.Set<BookingRequest>().Where(m => m.IsDeleted==false)
                .Include(m => m.Travel).Include(m => m.Customer).ToListAsync();
        }
    }
}
