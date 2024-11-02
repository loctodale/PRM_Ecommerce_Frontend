using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository() { }
        public OrderDetailRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<OrderDetail>> GetByOrderId(Guid id)
        {
            return await _context.OrderDetails.Include(x => x.KoiFish).ThenInclude(y => y.Category).Where(x => x.KoiOrderId == id).ToListAsync();
        }

        public async Task UpdateRange(List<OrderDetail> listOrder)
        {
            _context.OrderDetails.UpdateRange(listOrder);
            _context.SaveChanges();
        }
    }
}
