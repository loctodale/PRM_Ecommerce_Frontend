using KoiOrderingSystemInJapan.Data.Base;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;
using static KoiOrderingSystemInJapan.Data.ConstEnum;

namespace KoiOrderingSystemInJapan.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() { }
        public UserRepository(KoiOrderingSystemInJapanContext context) => _context = context;

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetByRole(int roleNum)
        {
            var role = (ConstEnum.Role)roleNum;
            return await _context.Users.Where(e => e.Role == role).ToListAsync();
        }

        public async Task<User?> FindByEmailOrUsername(string keyword)
        {
            var queryable = _context.Set<User>().AsQueryable();

            var user = await queryable.Where(e => e.Email!.ToLower().Trim() == keyword.ToLower().Trim()
                                                  || e.Username!.ToLower().Trim() == keyword.ToLower().Trim())
                .SingleOrDefaultAsync();

            return user;
        }
    }
}
