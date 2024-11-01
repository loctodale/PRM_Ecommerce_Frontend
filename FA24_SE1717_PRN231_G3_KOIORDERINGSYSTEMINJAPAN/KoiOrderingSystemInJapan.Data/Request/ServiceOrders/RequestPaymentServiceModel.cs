namespace KoiOrderingSystemInJapan.Data.Request.ServiceOrders
{
    public class RequestPaymentServiceModel
    {
        public Guid BookingRequestId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
