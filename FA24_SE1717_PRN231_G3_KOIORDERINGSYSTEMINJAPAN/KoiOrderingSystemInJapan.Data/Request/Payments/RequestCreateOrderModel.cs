namespace KoiOrderingSystemInJapan.Data.Request.Payments
{
    public class RequestCreateOrderModel
    {
        public Guid OrderId { get; set; }
        public string OrderType { get; set; }
        public decimal Price { get; set; }
        public DateTime Buy_date { get; set; }
    }
}
