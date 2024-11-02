using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.MVCWebApp.Tools;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: Travels
        public async Task<IActionResult> Index()
        {
            var travels = await GetTravelsAsync();
            var koiFishs = await GetKoiFishsAsync();

            ViewBag.Travels = travels;
            ViewBag.KoiFishs = koiFishs;

            return View();
        }

        public async Task<IActionResult> CaKoiNhat(string category)
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

        public async Task<IActionResult> ChuyenDiJapan()
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

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
