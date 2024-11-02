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
using Newtonsoft.Json;
using KoiOrderingSystemInJapan.Service.Base;
using Azure;
using Microsoft.AspNetCore.Identity;
/*using static KoiOrderingSystemInJapan.Data.Enum;*/

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class DeliveriesController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;
     

        public DeliveriesController()
        {
          
        }
        public async Task<IActionResult> DeliveryUser()
        {
            return View();
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Deliveries"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Delivery>>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Delivery>());
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            /*   using (var httpClient = new HttpClient())
               {
                   using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Deliveries/" + id))
                   {
                       if (response.IsSuccessStatusCode)
                       {
                           var content = await response.Content.ReadAsStringAsync();
                           var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                           if (result != null && result.Data != null)
                           {
                               var data = JsonConvert.DeserializeObject<List<Delivery>>(result.Data.ToString());
                               return View(data);
                           }
                       }
                   }
               }
               return View(new Delivery());*/
            return View(id);
        }

        // GET: Deliveries/Create
        public async Task<IActionResult> Create()
        {
            using (var httpClient = new HttpClient())
            {
                using (var koiOrderResponse = await httpClient.GetAsync(Const.APIEndPoint + "KoiOrders"))
                {
                    if (koiOrderResponse.IsSuccessStatusCode)
                    {
                        var content = await koiOrderResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<KoiOrder>>(result.Data.ToString());
                            ViewBag.KoiOrderId = new SelectList(data,"Id" , "Id");
                        }
                        else
                        {
                            ViewBag.KoiOrderId = new SelectList(new List<KoiOrder>());
                        }
                    }
                    else
                    {
                        // Handle error
                        ViewBag.KoiOrderId = new SelectList(new List<KoiOrder>());
                    }
                }
            }
            using (var httpClient = new HttpClient())
            {
                using (var deliveryStaffResponse = await httpClient.GetAsync(Const.APIEndPoint + "Users"))
                {
                    if (deliveryStaffResponse.IsSuccessStatusCode)
                    {
                        var content = await deliveryStaffResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());
                            var deliveryStaff = data.Where(u => u.Role == Data.ConstEnum.Role.Deliver).ToList();
                            if (deliveryStaff.Any())
                            {
                                ViewBag.DeliveryStaffId = new SelectList(data, "Id", "Lastname");
                            }
                            else
                            {
                                ViewBag.DeliveryStaffId = new SelectList(new List<User>(), "Id", "Lastname");
                            }
                        }
                    }
                    else
                    {
                        // Handle error
                        ViewBag.DeliveryStaffId = new SelectList(new List<User>(), "Id", "Lastname");
                    }
                }
            }
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      /*  [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KoiOrderId,DeliveryStaffId,Code,Name,Phone,Address,TotalAmount,PaymentReceived,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] Delivery delivery)
        {
            #region Save
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Deliveries/", delivery))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                return View(result);
                            }
                            else
                            {
                                return View(delivery);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
            #endregion
        }*/

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid) {
             using (var https = new HttpClient())
                {
                    using(var response = await https.GetAsync(Const.APIEndPoint+ "Deliveries/" + id))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result =  JsonConvert.DeserializeObject<BusinessResult>(content);
                            if(result!= null && result.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<Delivery>(result.Data.ToString());
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

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,KoiOrderId,DeliveryStaffId,Code,Name,Phone,Address,TotalAmount,PaymentReceived,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] Delivery delivery)
        {
            if (id != delivery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Deliveries/", delivery))
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
                                return View(delivery);
                            }
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
         

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Deliveries/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<Delivery>(result.Data.ToString());
                            return View(data);

                        }
                    }
                }
            }
            return RedirectToPage("Index");
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var koiOrderResponse = await httpClient.DeleteAsync(Const.APIEndPoint + "Deliveries/" + id))
                {
                    if (koiOrderResponse.IsSuccessStatusCode)
                    {
                        var content = await koiOrderResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Message == Const.SUCCESS_DELETE_MSG)
                        {
                            TempData["SuccessMessage"] = "Order deleted successfully.";
                            return RedirectToAction(nameof(Index)); // hoặc trả về view nào đó
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to delete order.";
                            return RedirectToAction(nameof(Index)); // Hoặc view lỗi
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        private bool DeliveryExists(Guid id)
        {
            return _context.Deliveries.Any(e => e.Id == id);
        }
    }
}
