using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class DeliveryRepository : GenericRepository<Delivery>
    {
        public DeliveryRepository() { }
        public DeliveryRepository(KoiOrderingSystemInJapanContext context) {
        _context = context;
        }


        public async Task<List<Delivery>> GetAll()
        {
            return await _context.Deliveries.Include(x => x.KoiOrder).Include(y=>y.DeliveryStaff).ToListAsync();
        }

        public async Task<int> UpdateTesting(Delivery delivery)
        {
            // Check if the entity with the same Id is already tracked in the context
            var existingEntity = _context.Deliveries.Local.FirstOrDefault(e => e.Id == delivery.Id);

            if (existingEntity != null)
            {
                // Detach the already tracked entity to avoid conflict
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach and mark the entity as modified
            var tracker = _context.Attach(delivery);
            tracker.State = EntityState.Modified;

            // Save changes to the database
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            delivery.IsDeleted = true;
            // Check if the entity with the same Id is already tracked in the context
            var existingEntity = _context.Deliveries.Local.FirstOrDefault(e => e.Id == delivery.Id);

            if (existingEntity != null)
            {
                // Detach the already tracked entity to avoid conflict
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach and mark the entity as modified
            var tracker = _context.Attach(delivery);
            tracker.State = EntityState.Modified;

            // Save changes to the database
            return await _context.SaveChangesAsync() >0;
        }

        public async Task<(List<Delivery> Item, int TotalPages)> SearchDelivery(string? deliveryName, string? code, string? location, int page, int pagesize)
        {
            var deliverylist = await _context.Deliveries.ToListAsync();
            if(deliveryName!= null)
            {
                deliverylist = deliverylist.Where(x=> x.Name.StartsWith(deliveryName)).ToList();
            } 
            if(code != null)
            {
                deliverylist = deliverylist.Where(x=> x.Code.StartsWith(code)).ToList();
            }
            if(location != null)
            {
                deliverylist = deliverylist.Where(x=> x.Address.StartsWith(location)).ToList();
            }

            var totalitems = deliverylist.Count();
            var totalPage = (int)Math.Ceiling(totalitems/(double)pagesize);
            var items = deliverylist.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            return (items, totalPage);
        }

    }
}
 