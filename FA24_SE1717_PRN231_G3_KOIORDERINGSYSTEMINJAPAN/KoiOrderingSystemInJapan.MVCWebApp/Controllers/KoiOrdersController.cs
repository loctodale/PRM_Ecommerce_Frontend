using KoiOrderingSystemInJapan.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class KoiOrdersController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public KoiOrdersController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: KoiOrders
        public async Task<IActionResult> Index()
        {
            //var koiOrderingSystemInJapanContext = _context.KoiOrders.Include(k => k.Customer).Include(k => k.Invoice);
            //return View(await koiOrderingSystemInJapanContext.ToListAsync());
            return View();
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return View(id);
        }

        // GET: KoiOrders/Details/5
        // public async Task<IActionResult> Details(Guid? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var koiOrder = await _context.KoiOrders
        //         .Include(k => k.Customer)
        //         .Include(k => k.Invoice)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (koiOrder == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(koiOrder);
        // }

        // GET: KoiOrders/Create
        // public IActionResult Create()
        // {
        //     ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
        //     ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id");
        //     return View();
        // }

        // POST: KoiOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,CustomerId,InvoiceId,Quantity,TotalPrice,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] KoiOrder koiOrder)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         koiOrder.Id = Guid.NewGuid();
        //         _context.Add(koiOrder);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", koiOrder.CustomerId);
        //     ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", koiOrder.InvoiceId);
        //     return View(koiOrder);
        // }

        // GET: KoiOrders/Edit/5
        // public async Task<IActionResult> Edit(Guid? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var koiOrder = await _context.KoiOrders.FindAsync(id);
        //     if (koiOrder == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", koiOrder.CustomerId);
        //     ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", koiOrder.InvoiceId);
        //     return View(koiOrder);
        // }

        // POST: KoiOrders/Edit/5
        //  To protect from overposting attacks, enable the specific properties you want to bind to.
        //  For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(Guid id, [Bind("Id,CustomerId,InvoiceId,Quantity,TotalPrice,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] KoiOrder koiOrder)
        // {
        //     if (id != koiOrder.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(koiOrder);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!KoiOrderExists(koiOrder.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", koiOrder.CustomerId);
        //     ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", koiOrder.InvoiceId);
        //     return View(koiOrder);
        // }

        // GET: KoiOrders/Delete/5
        // public async Task<IActionResult> Delete(Guid? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var koiOrder = await _context.KoiOrders
        //         .Include(k => k.Customer)
        //         .Include(k => k.Invoice)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (koiOrder == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(koiOrder);
        // }

        // POST: KoiOrders/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(Guid id)
        // {
        //     var koiOrder = await _context.KoiOrders.FindAsync(id);
        //     if (koiOrder != null)
        //     {
        //         _context.KoiOrders.Remove(koiOrder);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool KoiOrderExists(Guid id)
        // {
        //     return _context.KoiOrders.Any(e => e.Id == id);
        // }
    }
}
