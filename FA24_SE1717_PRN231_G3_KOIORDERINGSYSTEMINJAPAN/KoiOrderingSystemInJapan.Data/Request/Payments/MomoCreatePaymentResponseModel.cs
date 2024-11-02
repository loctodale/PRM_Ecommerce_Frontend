namespace KoiOrderingSystemInJapan.Data.Request.Payments
{
    public class MomoCreatePaymentResponseModel
    {
        public string PartnerCode { get; set; }
        public string RequestId { get; set; }
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public long ResponseTime { get; set; }
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public string PayUrl { get; set; }
        public string ShortLink { get; set; }
    }
}
