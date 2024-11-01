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
    public class ChuyenDiJapanController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public ChuyenDiJapanController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: ChuyenDiJapan
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

        // GET: ChuyenDiJapan/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Travels/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Travel>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new Travel());
        }

        // GET: ChuyenDiJapan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChuyenDiJapan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Price,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] Travel travel)
        {
            if (ModelState.IsValid)
            {
                travel.Id = Guid.NewGuid();
                _context.Add(travel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(travel);
        }

        // GET: ChuyenDiJapan/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }
            return View(travel);
        }

        // POST: ChuyenDiJapan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Location,Price,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] Travel travel)
        {
            if (id != travel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelExists(travel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(travel);
        }

        // GET: ChuyenDiJapan/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travel == null)
            {
                return NotFound();
            }

            return View(travel);
        }

        // POST: ChuyenDiJapan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var travel = await _context.Travels.FindAsync(id);
            if (travel != null)
            {
                _context.Travels.Remove(travel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravelExists(Guid id)
        {
            return _context.Travels.Any(e => e.Id == id);
        }
    }
}
