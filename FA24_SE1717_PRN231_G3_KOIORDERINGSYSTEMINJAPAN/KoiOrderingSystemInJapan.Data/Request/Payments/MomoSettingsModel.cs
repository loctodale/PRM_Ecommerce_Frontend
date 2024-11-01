namespace KoiOrderingSystemInJapan.Data.Request.Payments
{
    public class MomoSettingsModel
    {
        public static MomoSettingsModel Instance { get; set; }
        public string MomoApiUrl { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string RedirectUrl { get; set; }
        public string PartnerCode { get; set; }
        public string RequestType { get; set; }
    }
}
