using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using KoiOrderingSystemInJapan.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly CategoryService _bookingRequestSerivce;

        public CategoriesController()
        {
            _bookingRequestSerivce ??= new CategoryService();
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IBusinessResult> GetCategories()
        {
            return await _bookingRequestSerivce.GetAll();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetCategory(Guid id)
        {
            var bookingRequest = await _bookingRequestSerivce.GetById(id);

            return bookingRequest;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutCategory(Category bookingRequest)
        {

            return await _bookingRequestSerivce.Save(bookingRequest);
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostCategory(Category bookingRequest)
        {
            return await _bookingRequestSerivce.Save(bookingRequest);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteCategory(Guid id)
        {
            return await _bookingRequestSerivce.DeleteById(id);
        }

    }
}
