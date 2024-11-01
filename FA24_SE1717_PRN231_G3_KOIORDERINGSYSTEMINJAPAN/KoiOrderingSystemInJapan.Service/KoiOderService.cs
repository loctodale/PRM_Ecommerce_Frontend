using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.KoiOrders;
using KoiOrderingSystemInJapan.Data.Request.Payments;
using KoiOrderingSystemInJapan.Service.Base;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IKoiOrderService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Save(KoiOrder koiOrder);
        Task<IBusinessResult> DeleteById(Guid id);
        Task<IBusinessResult> CreatePayment(RequestPaymentKoiOrderModel koiOrder);
        Task<IBusinessResult> GetByIdWithOrderDetail(Guid id);
        Task<IBusinessResult> UpdateOrder(RequestUpdateKoiOrderModel model);
        Task<IBusinessResult> SearchKoiOrder(string? customerName, decimal? price, int? quantity, int page, int pageSize);
    }
    public class KoiOrderService : IKoiOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public KoiOrderService()
        {
            _unitOfWork ??= new UnitOfWork();
            _paymentService ??= new PaymentService();
        }

        public async Task<IBusinessResult> DeleteById(Guid code)
        {
            try
            {
                var koiOrder = await _unitOfWork.KoiOrder.GetByIdAsync(code);
                if (koiOrder == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new KoiOrder());
                }
                else
                {
                    var result = await _unitOfWork.KoiOrder.RemoveAsync(koiOrder);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiOrder);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, koiOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            #region Business rule

            #endregion
            var koiOrder = await _unitOfWork.KoiOrder.GetAllAsync();
            if (koiOrder == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<KoiOrder>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiOrder);
            }
        }

        public async Task<IBusinessResult> GetById(Guid code)
        {
            var koiOrder = await _unitOfWork.KoiOrder.GetByIdAsync(code);
            if (koiOrder == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new KoiOrder());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiOrder);
            }
        }

        public async Task<IBusinessResult> SearchKoiOrder(string? customerName, decimal? price, int? quantity, int page = 1, int pageSize = 10)
        {
            var koiOrder = await _unitOfWork.KoiOrder.SearchKoiOrder(customerName, price, quantity, page, pageSize);
            var paginatedResult = new
            {
                Items = koiOrder.Items,
                TotalPages = koiOrder.TotalPages,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = koiOrder.Items.Count()
            };
            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, paginatedResult);
        }

        public async Task<IBusinessResult> Save(KoiOrder koiOrder)
        {
            try
            {
                int result = -1;
                var koiOrderTmp = _unitOfWork.KoiOrder.GetById(koiOrder.Id);

                if (koiOrderTmp == null)
                {
                    result = await _unitOfWork.KoiOrder.CreateAsync(koiOrder);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new KoiOrder());
                    }
                }
                else
                {
                    result = await _unitOfWork.KoiOrder.UpdateAsync(koiOrder);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new KoiOrder());
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreatePayment(RequestPaymentKoiOrderModel koiOrder)
        {
            try
            {
                var orderDetailList = new List<OrderDetail>();
                foreach (var items in koiOrder.OrderDetailList)
                {
                    orderDetailList.Add(new OrderDetail { Id = Guid.NewGuid(), KoiFishId = items.KoiFishId, Price = items.Price });
                }
                var koiOrderEntity = new KoiOrder
                {
                    Id = Guid.NewGuid(),
                    CustomerId = koiOrder.CustomerId,
                    InvoiceId = null,
                    Quantity = koiOrder.Quantity,
                    TotalPrice = koiOrder.TotalPrice,
                    Invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        PaymentAmount = koiOrder.TotalPrice,
                        PaymentDate = DateTime.Now
                    },
                    OrderDetails = orderDetailList
                };

                var koiOrderResult = await _unitOfWork.KoiOrder.CreateAsync(koiOrderEntity);
                if (koiOrderResult == 0)
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new KoiOrder());
                }
                var momorequest = new RequestCreateOrderModel
                {
                    Buy_date = DateTime.Now,
                    OrderId = koiOrderEntity.Id,
                    OrderType = "KoiOrder",
                    Price = (decimal)koiOrder.TotalPrice,
                };
                var result = await _paymentService.CreatePaymentAsync(momorequest);

                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateOrder(RequestUpdateKoiOrderModel model)
        {
            var foundKoiOrder = await this.GetById(model.Id);
            if (foundKoiOrder.Status == -1)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new KoiOrder());
            }
            var updateKoiOrder = (KoiOrder)foundKoiOrder.Data;
            updateKoiOrder.Note = model.NoteStatus;
            var updateResult = await this.Save(updateKoiOrder);
            if (updateResult.Status == -1)
            {
                return updateResult;
            }
            var foundOrderDetails = await _unitOfWork.OrderDetail.GetByOrderId(model.Id);

            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            foreach (var orderDetail in foundOrderDetails)
            {
                orderDetail.Note = model.NoteStatus;
                orderDetailList.Add(orderDetail);
            }
            await _unitOfWork.OrderDetail.UpdateRange(orderDetailList);
            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, foundKoiOrder);
        }

        public async Task<IBusinessResult> GetByIdWithOrderDetail(Guid id)
        {
            var koiOrder = _unitOfWork.KoiOrder.GetById(id);
            if (koiOrder == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new KoiOrder());
            }
            var orderDetail = await _unitOfWork.OrderDetail.GetByOrderId(id);
            koiOrder.OrderDetails = orderDetail;

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiOrder);
        }
    }
}
