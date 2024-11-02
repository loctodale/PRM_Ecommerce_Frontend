namespace KoiOrderingSystemInJapan.Data.Models
{
    public partial class ServiceXBookingRequest
    {
        public Guid ServiceId { get; set; }

        public Guid BookingRequestId { get; set; }

        public virtual Service? Service { get; set; }

        public virtual BookingRequest? BookingRequest { get; set; }
    }
}
