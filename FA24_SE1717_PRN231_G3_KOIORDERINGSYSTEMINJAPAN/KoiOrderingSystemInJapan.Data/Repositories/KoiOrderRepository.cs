using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class KoiOrderRepository : GenericRepository<KoiOrder>
    {
        public KoiOrderRepository() { }
        public KoiOrderRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<KoiOrder>> GetAllAsync()
        {
            return await _context.KoiOrders.Include(x => x.Customer).ToListAsync();
        }
        public async Task<KoiOrder> GetByIdAsync(Guid id)
        {
            return await _context.Set<KoiOrder>()
                .Include(x => x.OrderDetails)
                .ThenInclude(z => z.KoiFish)
                .ThenInclude(g => g.Category)
                .Include(y => y.Customer)
                .Include(h => h.Deliveries)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<(List<KoiOrder> Items, int TotalPages)> SearchKoiOrder(string? customerName, decimal? price, int? quantity, int page, int pageSize)
        {
            var koiOrderList = await _context.KoiOrders.Include(y => y.Customer).ToListAsync();
            if(customerName != null)
            {
                koiOrderList = koiOrderList.Where(x => x.Customer.Firstname == customerName).ToList();
            }
            if(price != null)
            {
                koiOrderList = koiOrderList.Where(x => x.TotalPrice == price).ToList();
            }
            if (quantity != null)
            {
                koiOrderList = koiOrderList.Where(x => x.Quantity == quantity).ToList();
            }
            var totalItems = koiOrderList.Count();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = koiOrderList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (items, totalPage);
        }
    }
}
