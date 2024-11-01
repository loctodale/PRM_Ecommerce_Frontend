using KoiOrderingSystemInJapan.Data.Request.Payments;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IPaymentService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(RequestCreateOrderModel order);
    }
    public class PaymentService : IPaymentService
    {
        public PaymentService() { }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(RequestCreateOrderModel order)
        {
            var requestId = "MMO" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + order.OrderId.ToString();
            var orderInfo = order.OrderType;
            CollectionLinkRequest request = new CollectionLinkRequest();
            request.orderInfo = orderInfo;
            request.partnerCode = "MOMO";
            request.ipnUrl = MomoSettingsModel.Instance.RedirectUrl;
            request.redirectUrl = MomoSettingsModel.Instance.RedirectUrl;
            request.amount = (long)order.Price;
            request.orderId = order.OrderId.ToString();
            request.requestId = requestId;
            request.requestType = MomoSettingsModel.Instance.RequestType;
            request.extraData = order.OrderType;
            request.partnerName = "test MoMo";
            request.storeId = "MoMoTestStore";
            request.orderGroupId = "";
            request.autoCapture = true;
            request.lang = "vi";
            request.orderExpireTime = 30;

            var rawSignature = "accessKey=" + MomoSettingsModel.Instance.AccessKey + "&amount=" + request.amount + "&extraData=" + request.extraData + "&ipnUrl=" + request.ipnUrl + "&orderId=" + request.orderId + "&orderInfo=" + request.orderInfo + "&partnerCode=" + request.partnerCode + "&redirectUrl=" + request.redirectUrl + "&requestId=" + request.requestId + "&requestType=" + request.requestType;
            request.signature = ComputeHmacSha256(rawSignature, MomoSettingsModel.Instance.SecretKey);

            var client = new RestClient(MomoSettingsModel.Instance.MomoApiUrl);
            var momorequest = new RestRequest() { Method = Method.Post };
            momorequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
            momorequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);
            var response = await client.ExecuteAsync(momorequest);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }
    }
}
