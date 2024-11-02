using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class BookingRequestsController : Controller
    {
        private readonly HttpClient _http;

        public BookingRequestsController()
        {
        }

        #region Queries
        // GET: BookingRequests
        public async Task<IActionResult> Index()
        {
            var data = await GetBookingRequestsAsync();

            return View(data);
        }

        private async Task<List<BookingRequest>> GetBookingRequestsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "BookingRequests"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<BookingRequest>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<BookingRequest>();
        }

        private async Task<List<User>> GetCustomersAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Users"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<User>();
        }

        private async Task<List<Travel>> GetTravelsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Travels"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Travel>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<Travel>();
        }

        private async Task<BookingRequest> GetBookingRequestByIdAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "BookingRequests/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<BookingRequest>(result.Data.ToString());
                            return data;

                        }
                    }
                }
            }
            return new BookingRequest();
        }

        #endregion

        // GET: BookingRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetBookingRequestByIdAsync(id.Value);

            return View(data);
        }

        // GET: BookingRequests/Create
        public async Task<IActionResult> Create()
        {
            var status = Enum.GetValues(typeof(ConstEnum.BookingRequestStatus))
                     .Cast<ConstEnum.BookingRequestStatus>()
                     .Select(s => new
                     {
                         Value = s.ToString(), // Giá trị được gửi về khi chọn
                         Text = s.ToString()   // Tên hiển thị trong dropdown
                     })
                     .ToList();

            ViewBag.StatusList = new SelectList(status, "Value", "Text");

            var customers = await GetCustomersAsync();
            ViewBag.CustomerId = new SelectList(customers, "Id", "Username");

            var travels = await GetTravelsAsync();
            ViewBag.TravelId = new SelectList(travels, "Id", "Name"); 

            return View();
        }

        // POST: BookingRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingRequest bookingRequest)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "BookingRequests/", bookingRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            return View(bookingRequest);
        }

        // GET: BookingRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var data = await GetBookingRequestByIdAsync(id.Value);

            var status = Enum.GetValues(typeof(ConstEnum.BookingRequestStatus))
                     .Cast<ConstEnum.BookingRequestStatus>()
                     .Select(s => new
                     {
                         Value = s.ToString(), // Giá trị được gửi về khi chọn
                         Text = s.ToString()   // Tên hiển thị trong dropdown
                     })
                     .ToList();

            ViewBag.StatusList = new SelectList(status, "Value", "Text", data.Status.ToString());

            var customers = await GetCustomersAsync();
            ViewBag.CustomerId = new SelectList(customers, "Id", "Username", data?.CustomerId); 

            var travels = await GetTravelsAsync();
            ViewBag.TravelId = new SelectList(travels, "Id", "Name", data?.TravelId); 

            return View(data);
        }

        // POST: BookingRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BookingRequest bookingRequest)
        {
            if (id != bookingRequest.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "BookingRequests/" + id, bookingRequest))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            return View(bookingRequest);
        }

        // GET: BookingRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetBookingRequestByIdAsync(id.Value);

            return View(data);
        }

        // POST: BookingRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bookingRequest = await GetBookingRequestByIdAsync(id);
            if (bookingRequest != null)
            {
                // remove
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "BookingRequests/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }

            return View(bookingRequest);
        }

    }
}
