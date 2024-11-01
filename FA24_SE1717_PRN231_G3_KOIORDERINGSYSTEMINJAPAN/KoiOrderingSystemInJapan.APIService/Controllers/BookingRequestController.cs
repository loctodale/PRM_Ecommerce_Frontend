using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using KoiOrderingSystemInJapan.Service;
using Microsoft.AspNetCore.Mvc;
using KoiOrderingSystemInJapan.Data.Request.BookingRequests;
namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingRequestsController : ControllerBase
    {

        private readonly IBookingRequestService _bookingRequestSerivce;

        public BookingRequestsController()
        {
            _bookingRequestSerivce ??= new BookingRequestService();
        }

        [HttpGet]
        public async Task<IBusinessResult> GetBookingRequests()
        {
            return await _bookingRequestSerivce.GetAll();
        }

        // GET: api/BookingRequests
        [HttpPost("filter")]
        public async Task<IBusinessResult> GetBookingRequests([FromBody] BookingRequestRequest query)
        {
            return await _bookingRequestSerivce.GetAll(query, query.Page, query.PageSize);
        }

        [HttpGet("get-all")]
        public async Task<IBusinessResult> GetAllNoFilter()
        {
            return await _bookingRequestSerivce.GetAllNoFilter();
        }

        [HttpGet("with-no-sale")]
        public async Task<IBusinessResult> GetBookingRequestsWithNoSale()
        {
            return await _bookingRequestSerivce.GetBookingRequestsWithNoSale();
        }

        // GET: api/BookingRequests/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetBookingRequest(Guid id)
        {
            var bookingRequest = await _bookingRequestSerivce.GetById(id);

            return bookingRequest;
        }

        // PUT: api/BookingRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutBookingRequest(BookingRequest bookingRequest)
        {

            return await _bookingRequestSerivce.Save(bookingRequest);
        }

        // POST: api/BookingRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostBookingRequest(BookingRequest bookingRequest)
        {
            return await _bookingRequestSerivce.Save(bookingRequest);
        }

        // DELETE: api/BookingRequests/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteBookingRequest(Guid id)
        {
            return await _bookingRequestSerivce.DeleteById(id);
        }

    }
}
