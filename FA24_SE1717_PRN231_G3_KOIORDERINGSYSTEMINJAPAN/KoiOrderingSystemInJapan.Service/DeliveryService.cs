using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IDeliveryService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Create(Delivery user);
        Task<IBusinessResult> Update(Delivery user);
        Task<IBusinessResult> Save(Delivery user);
        Task<IBusinessResult> DeleteById(Guid id);
        Task<IBusinessResult> SearchDelivery(string? deliveryName, string? code, string? location, int page = 1, int pageSize = 10);
    }
    public class DeliveryService : IDeliveryService
    {
        private readonly UnitOfWork _unitOfWork;
        public DeliveryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public Task<IBusinessResult> Create(Delivery user)
        {
            throw new NotImplementedException();
        }

        public async Task<IBusinessResult> DeleteById(Guid id)
        {
            try
            {
                var delivery = await _unitOfWork.Delivery.GetByIdAsync(id);
                if (delivery != null)
                {
                    var result = await _unitOfWork.Delivery.Delete(delivery.Id);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, result);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            var result = await _unitOfWork.Delivery.GetAll();
            if (result != null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            else
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<IBusinessResult> GetById(Guid id)
        {
            var result = await _unitOfWork.Delivery.GetByIdAsync(id);
            if (result != null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            else
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<IBusinessResult> Save(Delivery delivery)
        {
            try
            {
                int result = -1;
                var invoiceTmp = _unitOfWork.Delivery.GetById(delivery.Id);

                if (invoiceTmp == null)
                {
                    result = await _unitOfWork.Delivery.CreateAsync(delivery);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Delivery());
                    }
                }
                else
                {
                    result = await _unitOfWork.Delivery.UpdateTesting(delivery);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Delivery());
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> SearchDelivery(string? deliveryName, string? code, string? location , int page = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Delivery.SearchDelivery(deliveryName, code, location , page, pageSize);
            var paginatedResult = new
            {
                Items = result.Item,
                TotalPages = result.TotalPages,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = result.Item.Count() // use totalitems from your repository method
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, paginatedResult);
           
        }

        public Task<IBusinessResult> Update(Delivery user)
        {
            throw new NotImplementedException();
        }
    }
}
