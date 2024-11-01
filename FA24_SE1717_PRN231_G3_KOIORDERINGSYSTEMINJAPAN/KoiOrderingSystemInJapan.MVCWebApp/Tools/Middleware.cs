using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Auths;
using KoiOrderingSystemInJapan.Service.Base;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace KoiOrderingSystemInJapan.MVCWebApp.Tools
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var categories = await GetCategoriesAsync();
            httpContext.Items["KoiCategories"] = categories;

            // Kiểm tra token từ cookie
            if (httpContext.Request.Cookies.TryGetValue("token", out var token))
            {
                var user = await Helper.GetCurrentUserAsync(httpContext);
                if (user != null)
                {
                    httpContext.Items["IsLogin"] = true;
                    httpContext.Items["User"] = user;

                    // Kiểm tra quyền
                    httpContext.Items["IsGuest"] = user.Role == ConstEnum.Role.Customer;
                }
                else
                {
                    // Token không hợp lệ hoặc người dùng không tồn tại
                    httpContext.Items["IsLogin"] = false;
                    httpContext.Items["IsGuest"] = true;
                }
            }
            else
            {
                // Không có token, người dùng là khách
                httpContext.Items["IsGuest"] = true;
                httpContext.Items["IsLogin"] = false;
            }

            await _next(httpContext);
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
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
    public class DecodedToken
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public long? Exp { get; set; }
    }

}
