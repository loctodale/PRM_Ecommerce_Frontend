using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.Auths
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? Expiration { get; set; }

        public LoginResponse()
        {
        }

        public LoginResponse(string? token, string? expiration)
        {
            Token = token;
            Expiration = expiration;
        }
    }
}
