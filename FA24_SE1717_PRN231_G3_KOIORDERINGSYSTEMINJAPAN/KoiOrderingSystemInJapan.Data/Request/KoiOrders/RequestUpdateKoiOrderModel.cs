using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.KoiOrders
{
    public class RequestUpdateKoiOrderModel
    {
        public Guid Id { get; set; }
        public string NoteStatus { get; set; }
    }
}
