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
    public class CaKoiNhatController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public CaKoiNhatController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: CaKoiNhat
        public async Task<IActionResult> Index(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                var data = await GetKoiFishsAsync();
                return View(data);
            }
            else
            {
                var data = await GetKoiFishsByCategoryAsync(category);
                return View(data);
            }
        }

        private async Task<List<KoiFish>> GetKoiFishsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "KoiFishes"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<KoiFish>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<KoiFish>();
        }

        private async Task<List<KoiFish>> GetKoiFishsByCategoryAsync(string category)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Const.APIEndPoint}KoiFishes/category?name={category}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<KoiFish>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<KoiFish>();
        }

        // GET: CaKoiNhat/Details/5
        public async Task<IActionResult> Details(Guid? id)
            {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "KoiFishes/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<KoiFish>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new KoiFish());
        }

        // GET: CaKoiNhat/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: CaKoiNhat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dob,Name,CategoryId,Picture,Price,Description,Origin,Status,DateSold,Gender,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] KoiFish koiFish)
        {
            if (ModelState.IsValid)
            {
                koiFish.Id = Guid.NewGuid();
                _context.Add(koiFish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", koiFish.CategoryId);
            return View(koiFish);
        }

        // GET: CaKoiNhat/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koiFish = await _context.KoiFishes.FindAsync(id);
            if (koiFish == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", koiFish.CategoryId);
            return View(koiFish);
        }

        // POST: CaKoiNhat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Dob,Name,CategoryId,Picture,Price,Description,Origin,Status,DateSold,Gender,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] KoiFish koiFish)
        {
            if (id != koiFish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(koiFish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KoiFishExists(koiFish.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", koiFish.CategoryId);
            return View(koiFish);
        }

        // GET: CaKoiNhat/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koiFish = await _context.KoiFishes
                .Include(k => k.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koiFish == null)
            {
                return NotFound();
            }

            return View(koiFish);
        }

        // POST: CaKoiNhat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var koiFish = await _context.KoiFishes.FindAsync(id);
            if (koiFish != null)
            {
                _context.KoiFishes.Remove(koiFish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KoiFishExists(Guid id)
        {
            return _context.KoiFishes.Any(e => e.Id == id);
        }
    }
}
