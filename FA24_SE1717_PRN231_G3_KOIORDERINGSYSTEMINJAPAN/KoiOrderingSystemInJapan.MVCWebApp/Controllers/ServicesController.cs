using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using ServiceEntity = KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Service.Base;
using Newtonsoft.Json;


namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly HttpClient _http;

        public ServicesController()
        {
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var data = await GetServicesAsync();

            return View(data);
        }

        private async Task<List<ServiceEntity.Service>> GetServicesAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Services"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<ServiceEntity.Service>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<ServiceEntity.Service>();
        }
        private async Task<ServiceEntity.Service> GetServiceByIdAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<ServiceEntity.Service>(result.Data.ToString());
                            return data;

                        }
                    }
                }
            }
            return new ServiceEntity.Service();
        }


        // GET: Services/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var data = await GetServiceByIdAsync(id.Value);
            return View(data);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceEntity.Service service)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Services/", service))
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

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var data = await GetServiceByIdAsync(id.Value);

            return View(data);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ServiceEntity.Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Services/" + id, service))
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

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetServiceByIdAsync(id.Value);

            return View(data);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var service = await GetServiceByIdAsync(id);
            if (service != null)
            {
                // remove
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "Services/" + id))
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
