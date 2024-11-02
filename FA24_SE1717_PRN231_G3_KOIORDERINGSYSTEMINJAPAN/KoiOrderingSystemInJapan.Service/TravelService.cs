using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiOrderingSystemInJapan.Data.Request.Travels;

namespace KoiOrderingSystemInJapan.Service
{
    public interface ITravelService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetAll(TravelRequest request, int page, int pageSize);
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Save(Travel travel);
        Task<IBusinessResult> DeleteById(Guid id);
    }
    public class TravelService : ITravelService
    {
        private readonly UnitOfWork _unitOfWork;
        public TravelService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteById(Guid code)
        {
            try
            {
                var travel = await _unitOfWork.Travel.GetByIdAsync(code);
                if (travel == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Travel());
                }
                else
                {
                    var result = await _unitOfWork.Travel.RemoveAsync(travel);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, travel);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, travel);
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
            var travel = await _unitOfWork.Travel.GetAllAsync();
            if (travel == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Travel>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, travel);
            }
        }

        public async Task<IBusinessResult> GetTravelBySearchKeyAsync(string searchKey)
        {
            #region Business rule

            #endregion
            var travel = await _unitOfWork.Travel.GetTravelBySearchKeyAsync(searchKey);
            if (travel == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Travel>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, travel);
            }
        }

        public async Task<IBusinessResult> GetById(Guid code)
        {
            var travel = await _unitOfWork.Travel.GetByIdAsync(code);
            if (travel == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new Travel());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, travel);
            }
        }

        public async Task<IBusinessResult> Save(Travel travel)
        {
            try
            {
                int result = -1;
                var travelTmp = _unitOfWork.Travel.GetById(travel.Id);

                if (travelTmp == null)
                {
                    result = await _unitOfWork.Travel.CreateAsync(travel);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Travel());
                    }
                }
                else
                {
                    _unitOfWork.Travel.Context().Entry(travelTmp).CurrentValues.SetValues(travel);
                    result = await _unitOfWork.Travel.UpdateAsync(travelTmp);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Travel());
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll(TravelRequest request, int page, int pageSize)
        {
            var item = await _unitOfWork.Travel.GetAllAsync(request, page, pageSize);
            var result = new
            {
                list = item.Item1,
                totalPages = item.Item2
            };
            if (result.list == null || !result.list.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, result);
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
        }
    }
}
