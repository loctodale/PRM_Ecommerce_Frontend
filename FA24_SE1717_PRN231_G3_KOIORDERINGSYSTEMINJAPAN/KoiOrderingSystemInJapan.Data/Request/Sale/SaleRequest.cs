using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.Sale
{
    public class SaleRequest
    {
        public string? ProposalDetails { get; set; }

        public decimal? TotalPrice { get; set; }

        public ConstEnum.StatusSale? Status { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string? ResponseBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 7;
    }
}
