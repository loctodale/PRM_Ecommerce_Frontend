using KoiOrderingSystemInJapan.Common.Tools;
using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.KoiFishs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class KoiFishRepository : GenericRepository<KoiFish>
    {
        public KoiFishRepository() { } 
        public KoiFishRepository(KoiOrderingSystemInJapanContext context) : base(context) { }

        public async Task<List<KoiFish>> GetAllAsync()
        {
            return await _context.Set<KoiFish>()
                .Include(m => m.Category)
                .Include(m => m.Size)
                .ToListAsync();
        }
        
        public async Task<List<KoiFish>> GetAllByCategoryAsync(string category)
        {
            var koiFishes = await _context.KoiFishes.Include(k => k.Category).Include(m => m.Size).ToListAsync(); 
            var koiFishByCategory = koiFishes
                .Where(k => SlugHelper.ConvertToSlugName(k.Category.Name) == category && !k.Category.IsDeleted)
                .ToList();
            return koiFishByCategory;
        }

        public async Task<(List<KoiFish>, int)> GetAllAsync(KoiFishRequest query, int page, int pageSize)
        {
            var queryable = _context.Set<KoiFish>().AsQueryable()
                .Include(m => m.Category).Include(m => m.Size)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(query.Name))
            {
                queryable = queryable.Where(m => m.Name.Trim().ToLower().Contains(query.Name.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Category?.Name))
            {
                queryable = queryable.Where(m => m.Category.Name.Trim().ToLower().Contains(query.Category.Name.Trim().ToLower()));
            }

            if (query.Price.HasValue)
            {
                queryable = queryable.Where(m => m.Price == query.Price);
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

        public async Task<KoiFish> GetByIdAsync(Guid id)
        {
            var query = _context.KoiFishes.AsQueryable();
            query = query.Include(k => k.Category).Include(m => m.Size).Where(m => m.Id == id);

            return await query.SingleOrDefaultAsync();
        }
    }

}
