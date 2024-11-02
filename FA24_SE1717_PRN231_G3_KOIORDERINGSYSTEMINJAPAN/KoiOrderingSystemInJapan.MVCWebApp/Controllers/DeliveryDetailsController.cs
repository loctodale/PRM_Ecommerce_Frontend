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
using Microsoft.VisualBasic;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class DeliveryDetailsController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public DeliveryDetailsController()
        {
        }


        // GET: DeliveryDetails
        public async Task<IActionResult> Index()
        {
            /*    using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.APIEndPoint + "DeliveryDetails"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<List<DeliveryDetail>>(result.Data.ToString());
                                return View(data);
                            }
                        }
                    }
                }
                return View(new List<Delivery>());*/
            return View();
        }

        // GET: DeliveryDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            /* if (id == null)
             {
                 return NotFound();
             }

             using (var httpClient = new HttpClient())
             {
                 using (var response = await httpClient.GetAsync(Const.APIEndPoint + "DeliveryDetails/" + id))
                 {
                     if (response.IsSuccessStatusCode)
                     {
                         var content = await response.Content.ReadAsStringAsync();
                         var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                         if (result != null)
                         {
                             var data = JsonConvert.DeserializeObject<DeliveryDetail>(result.Data.ToString());

                             return View(data);
                         }
                     }
                 }
             }
             return NotFound();*/
            return View(id);
        }

        // GET: DeliveryDetails/Create 
        public async Task<IActionResult> Create()
        {
            using (var httpClient = new HttpClient())
            {
                using (var koiOrderResponse = await httpClient.GetAsync(Const.APIEndPoint + "Deliveries"))
                {
                    if (koiOrderResponse.IsSuccessStatusCode)
                    {
                        var content = await koiOrderResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Delivery>>(result.Data.ToString());
                            ViewBag.DeliveryId = new SelectList(data, "Id", "Code");
                        }
                        else
                        {
                            ViewBag.DeliveryId = new SelectList(new List<Delivery>());
                        }
                    }
                    else
                    {
                        // Handle error
                        ViewBag.DeliveryId = new SelectList(new List<Delivery>());
                    }
                }
            }
            return View();
        }

        // POST: DeliveryDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*   [HttpPost]
           [ValidateAntiForgeryToken]
           public async Task<IActionResult> Create([Bind("Id,DeliveryId,Name,Description,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] DeliveryDetail deliveryDetail)
           {
               if (ModelState.IsValid)
               {
                   deliveryDetail.Id = Guid.NewGuid();
                   _context.Add(deliveryDetail);
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
               }
               ViewData["DeliveryId"] = new SelectList(_context.Deliveries, "Id", "Id", deliveryDetail.DeliveryId);
               return View(deliveryDetail);
           }*/

        // GET: DeliveryDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (var https = new HttpClient())
                {
                    using (var deliveryDetail = await https.GetAsync(Const.APIEndPoint + "DeliveryDetails/" + id))
                    {

                        if (deliveryDetail.IsSuccessStatusCode)
                        {
                            var content = await deliveryDetail.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<DeliveryDetail>(result.Data.ToString());
                                return View(data);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

            }
            return RedirectToAction("Index");


        }

        // POST: DeliveryDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DeliveryId,Name,Description,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] DeliveryDetail deliveryDetail)
        {
            if (id != deliveryDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var https = new HttpClient())
                {
                    using (var response = await https.PostAsJsonAsync(Const.APIEndPoint + "DeliveryDetails/", deliveryDetail))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                            {

                            }
                            else
                            {
                                return View(deliveryDetail);
                            }
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: DeliveryDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var koiOrderResponse = await httpClient.GetAsync(Const.APIEndPoint + "DeliveryDetails/" + id))
                {
                    if (koiOrderResponse.IsSuccessStatusCode)
                    {
                        var content = await koiOrderResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<DeliveryDetail>(result.Data.ToString());
                            return View(data);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to delete order.";
                            return RedirectToAction(nameof(Index)); // hoặc trả về view nào đó
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        // POST: DeliveryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var koiOrderResponse = await httpClient.DeleteAsync(Const.APIEndPoint + "DeliveryDetails/" + id))
                {
                    if (koiOrderResponse.IsSuccessStatusCode)
                    {
                        var content = await koiOrderResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Message == Const.SUCCESS_DELETE_MSG)
                        {
                            TempData["SuccessMessage"] = "Order deleted successfully.";
                            return RedirectToAction(nameof(Index));  // hoặc trả về view nào đó
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to delete order.";
                            return RedirectToAction(nameof(Index));  // hoặc trả về view nào đó
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }
    }
}
