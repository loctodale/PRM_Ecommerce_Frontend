namespace KoiOrderingSystemInJapan.Data.Request.KoiOrders
{
    public class RequestPaymentKoiOrderModel
    {
        public Guid? CustomerId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<OrderItemDetail> OrderDetailList { get; set; }
    }

    public class OrderItemDetail
    {
        public Guid? KoiFishId { get; set; }
        public decimal? Price { get; set; }
    }
}
