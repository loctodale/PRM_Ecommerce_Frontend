using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class ServiceRepository : GenericRepository<Service>
    {
        public ServiceRepository() { }

        public ServiceRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<(List<Service>, int)> GetAllAsync(ServiceRequest query, int page, int pageSize)
        {
            var queryable = _context.Set<Service>().AsQueryable();

            if (!string.IsNullOrEmpty(query.ServiceName))
            {
                queryable = queryable.Where(m => m.ServiceName.Trim().ToLower().Contains(query.ServiceName.Trim().ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Description))
            {
                queryable = queryable.Where(m => m.Description.Trim().ToLower().Contains(query.Description.Trim().ToLower()));
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

    }
}
