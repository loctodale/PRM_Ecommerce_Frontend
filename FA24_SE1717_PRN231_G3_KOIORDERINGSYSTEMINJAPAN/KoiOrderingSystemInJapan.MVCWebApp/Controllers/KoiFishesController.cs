using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Context;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.MVCWebApp.Tools;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class KoiFishesController : Controller
    {
        private readonly KoiOrderingSystemInJapanContext _context;

        public KoiFishesController(KoiOrderingSystemInJapanContext context)
        {
            _context = context;
        }

        // GET: KoiFishes
        //public async Task<IActionResult> Index()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync(Const.APIEndPoint + "KoiFishes"))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

        //                if (result != null && result.Data != null)
        //                {
        //                    var data = JsonConvert.DeserializeObject<List<KoiFish>>(result.Data.ToString());
        //                    return View(data);
        //                }
        //            }
        //        }
        //    }
        //    return View(new List<KoiFish>());
        //}

        public async Task<IActionResult> IndexAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                // Nếu không có category được truyền, lấy tất cả cá koi
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.APIEndPoint + "KoiFishes"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<List<KoiFish>>(result.Data.ToString());
                                return View(data);
                            }
                        }
                    }
                }
                return View(new List<KoiFish>());
            }
            else
            {
                // Nếu có category, lấy các koi dựa trên category
                var koiFishes = _context.KoiFishes.Include(k => k.Category).ToList(); // Lấy tất cả với category
                var koiFishByCategory = koiFishes
                    .Where(k => SlugHelper.ConvertToSlugName(k.Category.Name) == category && !k.Category.IsDeleted)
                    .ToList();

                if (koiFishByCategory == null || !koiFishByCategory.Any())
                {
                    return NotFound(); // Nếu không tìm thấy
                }

                return View(koiFishByCategory); // Trả dữ liệu về view
            }
        }
        // GET: KoiFishes/Details/5
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
        // GET: KoiFishes/Create
        public async Task<IActionResult> Create()
        {
            var categories = await GetCategoriesAsync();
            ViewBag.CategoryId = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View();
        }

        private async Task<List<Category>> GetCategoriesAsync()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Categories"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Category>>(result.Data.ToString());
                            return data;
                        }
                    }
                }
            }
            return new List<Category>();
        }

        // POST: KoiFishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KoiFish fish)
        {
            #region Save
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "KoiFishes/", fish))
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
            return View(fish);
            #endregion
        }
        // GET: Koifishes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var categories = await GetCategoriesAsync();
            ViewBag.CategoryId = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var data = await GetKoiFishByIdAsync(id.Value);

            return View(data);
        }

        private async Task<KoiFish> GetKoiFishByIdAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "KoiFishes/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null)
                        {
                            var data = JsonConvert.DeserializeObject<KoiFish>(result.Data.ToString());
                            return data;

                        }
                    }
                }
            }
            return new KoiFish();
        }


        // POST: KOiFishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, KoiFish fish)
        {
            if (id != fish.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "KoiFishes/", fish))
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
            return View(fish);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var data = await GetKoiFishByIdAsync(id.Value);

            return View(data);
        }

        // POST: KoiFishs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bookingRequest = await GetKoiFishByIdAsync(id);
            if (bookingRequest != null)
            {
                // remove
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "KoiFishes/" + id))
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

            return View(bookingRequest);
        }
    }
}
