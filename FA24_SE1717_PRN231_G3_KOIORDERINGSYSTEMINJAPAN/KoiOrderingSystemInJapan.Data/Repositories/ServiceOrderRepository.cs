using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Sale;
using KoiOrderingSystemInJapan.Data.Request.ServiceOrder;
using KoiOrderingSystemInJapan.Data.Response;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class ServiceOrderRepository : GenericRepository<ServiceOrder>
    {
        public ServiceOrderRepository() { }
        public ServiceOrderRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<ServiceOrder>> GetAllAsync()
        {
            return await _context.ServiceOrders.Include(e => e.Invoice)
                .Include(e => e.BookingRequest).ThenInclude(br => br.Customer)
                .Include(e => e.BookingRequest).ThenInclude(br => br.Travel).ToListAsync();
        }

        public async Task<PagedResult<ServiceOrder>> GetPagedServiceOrders(int page, int pageSize)
        {
            var totalItems = await _context.ServiceOrders.CountAsync(); 
            var data = await _context.ServiceOrders
                                     //.OrderBy(s => s.CreatedDate) // Sắp xếp theo ngày tạo (hoặc field khác)
                                     .Skip((page - 1) * pageSize) // Bỏ qua các bản ghi trước đó
                                     .Take(pageSize) // Lấy đúng số lượng bản ghi
                                     .ToListAsync(); // Thực thi truy vấn và lấy kết quả

            return new PagedResult<ServiceOrder>
            {
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<(List<ServiceOrder>, int)> GetAllAsync(ServiceOrderRequest query, int page, int pageSize)
        {
            var queryable = _context.Set<ServiceOrder>().AsQueryable();

            if (query.Quantity.HasValue)
            {
                queryable = queryable.Where(m => m.Quantity == query.Quantity);
            }

            if (query.TotalPrice.HasValue)
            {
                queryable = queryable.Where(m => m.TotalPrice == query.TotalPrice);
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

            queryable = queryable.Where(m => m.IsDeleted==false)
                .Include(e => e.Invoice)
                .Include(e => e.BookingRequest).ThenInclude(br => br.Customer)
                .Include(e => e.BookingRequest).ThenInclude(br => br.Travel);
            var data = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalPages);
        }

        public async Task<bool> UpdateIsDeleted(ServiceOrder serviceOrder)
        {
            serviceOrder.IsDeleted = true;
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                return true;
            }
            return false;
        }
    }
}
