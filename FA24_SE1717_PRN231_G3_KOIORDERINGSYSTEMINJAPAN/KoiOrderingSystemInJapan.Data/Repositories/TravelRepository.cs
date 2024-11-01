using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Travels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class TravelRepository : GenericRepository<Travel>
    {
        public TravelRepository() { }

        public TravelRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<Travel>> GetAllTravelAsync()
        {
            return await _context.Travels.Include(x => x.TravelFarms).ThenInclude( y => y.Farm).ToListAsync();
        }

        public async Task<(List<Travel>, int)> GetAllAsync(TravelRequest query, int page, int pageSize)
        {
            var queryable = _context.Set<Travel>()
        .Include(t => t.TravelFarms).ThenInclude(t => t.Farm) // Include để lấy dữ liệu từ bảng TravelFarms
        .AsQueryable()
        .AsNoTracking();

            //if (!string.IsNullOrEmpty(query.FarmName))
            //{
            //    // Lọc dữ liệu dựa trên FarmName từ bảng Farm
            //    queryable = queryable.Where(m => m.TravelFarms.Any(f =>
            //        f.Farm.Name.Trim().ToLower().Contains(query.Name.Trim().ToLower())));
            //}

            if (!string.IsNullOrEmpty(query.Name))
            {
                queryable = queryable.Where(m => m.Name.Trim().ToLower().Contains(query.Name.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Location))
            {
                queryable = queryable.Where(m => m.Location.Trim().ToLower().Contains(query.Location.Trim().ToLower()));
            }

            if (query.Price.HasValue)
            {
                queryable = queryable.Where(m => m.Price == query.Price);
            }

            // Base 
           

            var totalItems = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var data = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalPages);
        }

        public async Task<List<Travel>> GetTravelBySearchKeyAsync(string searchKey)
        {
            IQueryable<Travel> query = _context.Set<Travel>().AsQueryable();

            if (decimal.TryParse(searchKey, out decimal price))
            {
                query = query.Where(x => x.Price == price);
            }
            else
            {
                query = query.Include(x => x.TravelFarms).ThenInclude(y => y.Farm);
                query = query.Where(x => x.Name.Contains(searchKey) || x.TravelFarms.Any(f => f.Farm.Name.Contains(searchKey)));
            }

            return await query.ToListAsync();
        }

        public async Task<Travel> GetByIdAsync(Guid id)
        {
            var query = _context.Travels.AsQueryable();
            query = query.Where(m => m.Id == id);

            query = query.Include(k => k.TravelFarms).ThenInclude(m => m.Farm);

            return await query.SingleOrDefaultAsync();
        }
    }
}
