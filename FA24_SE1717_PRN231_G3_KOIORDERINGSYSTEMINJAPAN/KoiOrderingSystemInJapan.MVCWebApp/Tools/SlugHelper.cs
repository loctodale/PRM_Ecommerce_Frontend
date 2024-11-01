using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace KoiOrderingSystemInJapan.MVCWebApp.Tools
{
    
    public static class SlugHelper
    {
        public static string ConvertToSlugName(string name)
        {
            // Chuyển thành chữ thường
            string str = name.ToLowerInvariant();

            // Loại bỏ dấu tiếng Việt
            str = RemoveDiacritics(str);

            // Thay khoảng trắng bằng dấu gạch ngang
            str = Regex.Replace(str, @"\s+", "-");

            // Loại bỏ các ký tự đặc biệt
            str = Regex.Replace(str, @"[^a-z0-9-]", "");

            // Xóa dấu gạch ngang dư thừa nếu có
            str = Regex.Replace(str, @"-+", "-").Trim('-');

            return str;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

}
