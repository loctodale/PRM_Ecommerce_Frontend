using KoiOrderingSystemInJapan.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Data.Request.BookingRequests
{
    public class BookingRequestRequest : BaseFilterRequest
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? TravelId { get; set; }

        public int? QuantityService { get; set; }

        public int? NumberOfPerson { get; set; }

        public ConstEnum.BookingRequestStatus? Status { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string? Note { get; set; }

        public virtual User? Customer { get; set; }

        public virtual Travel? Travel { get; set; }

        
    }
}
