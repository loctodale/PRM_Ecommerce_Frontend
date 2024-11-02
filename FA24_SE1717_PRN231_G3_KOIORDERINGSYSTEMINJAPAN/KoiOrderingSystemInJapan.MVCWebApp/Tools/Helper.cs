using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Auths;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace KoiOrderingSystemInJapan.MVCWebApp.Tools
{
    public static class Helper
    {
        //public static async Task<bool> IsUserGuestAsync(string token)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Tạo đối tượng TokenRequest
        //        var tokenRequest = new TokenRequest { Token = token };

        //        // Chuyển đổi đối tượng thành JSON
        //        var jsonContent = JsonConvert.SerializeObject(tokenRequest);
        //        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //        // Gửi yêu cầu POST với nội dung JSON
        //        using (var response = await httpClient.PostAsync(Const.APIEndPoint + "Users/decode-token", content))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<BusinessResult>(responseContent);

        //                if (result != null && result.Data != null)
        //                {
        //                    // Kiểm tra dữ liệu trả về
        //                    var data = JsonConvert.DeserializeObject<DecodedToken>(result.Data.ToString());
        //                    if (data != null)
        //                    {
        //                        // Kiểm tra điều kiện để xác định xem người dùng có phải là guest hay không
        //                        return data.Name != null && data.Name.Equals("customer", StringComparison.OrdinalIgnoreCase);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    // Nếu không có phản hồi thành công hoặc không tìm thấy thông tin, coi như không phải guest
        //    return false;
        //}
        public static async Task<User> GetCurrentUserAsync(HttpContext httpContext)
        {
            var decodedToken = await DecodedTokenAsync(httpContext);

            if (decodedToken != null)
            {
                var user = await GetUserByIdAsync(decodedToken.Id);
                return user;
            }

            return null;
        }

        public static async Task<DecodedToken> DecodedTokenAvailable(HttpContext httpContext, string? token)
        {
            var tokenJson = new { Token = token};
            var jsonContent = JsonConvert.SerializeObject(tokenJson);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Const.APIEndPoint + "Users/decode-token", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(responseContent);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<DecodedToken>(result.Data.ToString());

                            // Kiểm tra thời gian hết hạn
                            if (data != null && data.Exp.HasValue)
                            {
                                var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(data.Exp.Value);
                                if (DateTimeOffset.UtcNow >= expirationDateTime)
                                {
                                    // Nếu token hết hạn, xoá cookie
                                    httpContext.Response.Cookies.Delete("token");
                                    return null;
                                }
                            }

                            return data;
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<DecodedToken> DecodedTokenAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.TryGetValue("token", out var token))
            {
                return null;
            }

            var tokenJson = new { Token = token.Replace("Bearer ", "") };
            var jsonContent = JsonConvert.SerializeObject(tokenJson);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(Const.APIEndPoint + "Users/decode-token", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(responseContent);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<DecodedToken>(result.Data.ToString());

                            // Kiểm tra thời gian hết hạn
                            if (data != null && data.Exp.HasValue)
                            {
                                var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(data.Exp.Value);
                                if (DateTimeOffset.UtcNow >= expirationDateTime)
                                {
                                    // Nếu token hết hạn, xoá cookie
                                    httpContext.Response.Cookies.Delete("token");
                                    return null;
                                }
                            }

                            return data;
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<User> GetUserByIdAsync(string userId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Users/" + userId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var userData = JsonConvert.DeserializeObject<User>(result.Data.ToString());
                            return userData;
                        }
                    }
                }
            }

            return null;
        }


    }
}
