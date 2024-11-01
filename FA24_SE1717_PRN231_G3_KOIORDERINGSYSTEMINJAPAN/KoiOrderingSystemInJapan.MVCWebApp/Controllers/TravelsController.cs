using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class TravelsController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public TravelsController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: Travels
        public async Task<IActionResult> Index()
        {
            var data = await GetTravelsAsync();

            return View(data);
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
        private async Task<Travel> GetTravelByIdAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Travels/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<Travel>(result.Data.ToString());
                            return data;

                        }
                    }
                }
            }
            return new Travel();
        }


        // GET: Travels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var data = await GetTravelByIdAsync(id.Value);
            return View(data);
        }

        // GET: Travels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Travels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Travel service)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Travels/", service))
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
            return View(service);
        }

        // GET: Travels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var data = await GetTravelByIdAsync(id.Value);

            return View(data);
        }

        // POST: Travels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Travel service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Travels/" + id, service))
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
            return View(service);
        }

        // GET: Travels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetTravelByIdAsync(id.Value);

            return View(data);
        }

        // POST: Travels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var service = await GetTravelByIdAsync(id);
            if (service != null)
            {
                // remove
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "Travels/" + id))
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

            return View(service);
        }
    }
}
