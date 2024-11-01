using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.ServiceOrder
{
    public class ServiceOrderRequest
    {
        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 7;
    }
}
