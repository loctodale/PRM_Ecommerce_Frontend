using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class DeliveryDetailRepository : GenericRepository<DeliveryDetail>
    {
        public DeliveryDetailRepository() { }
        public DeliveryDetailRepository(KoiOrderingSystemInJapanContext context) { }


        public async Task<List<DeliveryDetail>> GetAllById(Guid id)
        {
            var deliverydetails = await _context.DeliveryDetails.Where(x=> x.DeliveryId == id).ToListAsync(); 
            return deliverydetails;
        }

        public async Task<int> UpdateTesting(DeliveryDetail delivery) {
            // Check if the entity with the same Id is already tracked in the context
            var existingEntity = _context.DeliveryDetails.Local.FirstOrDefault(e => e.Id == delivery.Id);

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
            var delivery = await _context.DeliveryDetails.FindAsync(id);
            delivery.IsDeleted = true;
            // Check if the entity with the same Id is already tracked in the context
            var existingEntity = _context.DeliveryDetails.Local.FirstOrDefault(e => e.Id == delivery.Id);

            if (existingEntity != null)
            {
                // Detach the already tracked entity to avoid conflict
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach and mark the entity as modified
            var tracker = _context.Attach(delivery);
            tracker.State = EntityState.Modified;

            // Save changes to the database
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(List<DeliveryDetail> Items, int TotalPages)> SearchDeliveryDetail(string? deliverydetailName, bool? isdeleted, string? description , int page , int pagesize)
        {
            var deliverydetaillist = await _context.DeliveryDetails.Include(x=>x.Delivery).ToListAsync();
            if (deliverydetailName != null)
            {
                deliverydetaillist = deliverydetaillist.Where(x => x.Name.StartsWith(deliverydetailName)).ToList();
            }
            if (isdeleted != null)
            {
                deliverydetaillist = deliverydetaillist.Where(x => x.IsDeleted == isdeleted).ToList();
            }
            if (description != null)
            {
                deliverydetaillist = deliverydetaillist.Where(x => x.Description.StartsWith(description)).ToList();
            }

            var totalItem = deliverydetaillist.Count();
            var totalPage = (int)Math.Ceiling(totalItem / (double)pagesize);
            var items = deliverydetaillist.Skip((page - 1) * pagesize).Take(pagesize).ToList();    
            return (items, totalPage);
        }
    }
}
