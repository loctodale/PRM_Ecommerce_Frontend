  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;

namespace KoiOrderingSystemInJapan.MVCWebApp.Views
{
    public class KoiFishController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public KoiFishController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: KoiFish
        public async Task<IActionResult> Index()
        {
            var koiOrderingSystemInJapanContext = _context.KoiFishes.Include(k => k.Category);
            return View(await koiOrderingSystemInJapanContext.ToListAsync());
        }

        // GET: KoiFish/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: KoiFish/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: KoiFish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dob,CategoryId,Price,Description,Origin,Status,DateSold,Gender,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] KoiFish koiFish)
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

        // GET: KoiFish/Edit/5
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

        // POST: KoiFish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Dob,CategoryId,Price,Description,Origin,Status,DateSold,Gender,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] KoiFish koiFish)
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

        // GET: KoiFish/Delete/5
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

        // POST: KoiFish/Delete/5
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
