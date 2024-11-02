using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.Auths
{
    public class DecodedToken
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public long? Exp { get; set; }
    }
}
