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
    public class SalesController : Controller
    {
        private readonly HttpClient _http;

        public SalesController()
        {
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Sales"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Sale>>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View();
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Sales/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Sale>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View();
        }

        // GET: Sales/Create
        //public IActionResult Create()
        //{
        //    ViewData["BookingRequestId"] = new SelectList(_context.BookingRequests, "Id", "Id");
        //    ViewData["SaleStaffId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        //POST: Sales/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       //[HttpPost]
       // [ValidateAntiForgeryToken]
       // public async Task<IActionResult> Create([Bind("Id,BookingRequestId,SaleStaffId,ProposalDetails,TotalPrice,Status,ResponseDate,ResponseBy,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] Sale sale)
       // {
       //     if (ModelState.IsValid)
       //     {
       //         sale.Id = Guid.NewGuid();
       //         sale.CreatedBy = "";
       //         sale.CreatedDate = DateTime.Now;
       //         sale.UpdatedDate = DateTime.Now;
       //         sale.UpdatedBy = "";
       //         sale.Note = "";
       //         _context.Add(sale);
       //         await _context.SaveChangesAsync();
       //         return RedirectToAction(nameof(Index));
       //     }
       //     ViewData["BookingRequestId"] = new SelectList(_context.BookingRequests, "Id", "Id", sale.BookingRequestId);
       //     ViewData["SaleStaffId"] = new SelectList(_context.Users, "Id", "Id", sale.SaleStaffId);
       //     return View(sale);
       // }

        //// GET: Sales/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sale = await _context.Sales.FindAsync(id);
        //    if (sale == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BookingRequestId"] = new SelectList(_context.BookingRequests, "Id", "Id", sale.BookingRequestId);
        //    ViewData["SaleStaffId"] = new SelectList(_context.Users, "Id", "Id", sale.SaleStaffId);
        //    return View(sale);
        //}

        //// POST: Sales/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,BookingRequestId,SaleStaffId,ProposalDetails,TotalPrice,Status,ResponseDate,ResponseBy,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,Note")] Sale sale)
        //{
        //    if (id != sale.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(sale);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SaleExists(sale.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BookingRequestId"] = new SelectList(_context.BookingRequests, "Id", "Id", sale.BookingRequestId);
        //    ViewData["SaleStaffId"] = new SelectList(_context.Users, "Id", "Id", sale.SaleStaffId);
        //    return View(sale);
        //}

        //// GET: Sales/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sale = await _context.Sales
        //        .Include(s => s.BookingRequest)
        //        .Include(s => s.SaleStaff)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (sale == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sale);
        //}

        //// POST: Sales/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var sale = await _context.Sales.FindAsync(id);
        //    if (sale != null)
        //    {
        //        _context.Sales.Remove(sale);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SaleExists(Guid id)
        //{
        //    return _context.Sales.Any(e => e.Id == id);
        //}
    }
}
