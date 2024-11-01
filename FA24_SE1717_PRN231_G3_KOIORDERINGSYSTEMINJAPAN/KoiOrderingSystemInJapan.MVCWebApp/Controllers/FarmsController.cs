using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Service.Base;
using Newtonsoft.Json;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class FarmsController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public FarmsController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: Farms
        public async Task<IActionResult> Index()
        {
            var data = await GetFarmsAsync();

            return View(data);
        }

        private async Task<List<Farm>> GetFarmsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Farms"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Farm>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<Farm>();
        }
        private async Task<Farm> GetFarmByIdAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Farms/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<Farm>(result.Data.ToString());
                            return data;

                        }
                    }
                }
            }
            return new Farm();
        }


        // GET: Farms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var data = await GetFarmByIdAsync(id.Value);
            return View(data);
        }

        // GET: Farms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Farm service)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Farms/", service))
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

        // GET: Farms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var data = await GetFarmByIdAsync(id.Value);

            return View(data);
        }

        // POST: Farms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Farm service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Farms/" + id, service))
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

        // GET: Farms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetFarmByIdAsync(id.Value);

            return View(data);
        }

        // POST: Farms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var service = await GetFarmByIdAsync(id);
            if (service != null)
            {
                // remove
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "Farms/" + id))
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
