using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Service.Base;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IFarmService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Save(Farm farm);
        Task<IBusinessResult> DeleteById(Guid id);
    }
    public class FarmService : IFarmService
    {
        private readonly UnitOfWork _unitOfWork;
        public FarmService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteById(Guid code)
        {
            try
            {
                var farm = await _unitOfWork.Farm.GetByIdAsync(code);
                if (farm == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Farm());
                }
                else
                {
                    var result = await _unitOfWork.Farm.RemoveAsync(farm);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, farm);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, farm);
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
            var farm = await _unitOfWork.Farm.GetAllAsync();
            if (farm == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Farm>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, farm);
            }
        }

        public async Task<IBusinessResult> GetById(Guid code)
        {
            var farm = await _unitOfWork.Farm.GetByIdAsync(code);
            if (farm == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new Farm());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, farm);
            }
        }

        public async Task<IBusinessResult> Save(Farm farm)
        {
            try
            {
                int result = -1;
                var farmTmp = _unitOfWork.Farm.GetById(farm.Id);

                if (farmTmp == null)
                {
                    result = await _unitOfWork.Farm.CreateAsync(farm);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Farm());
                    }
                }
                else
                {
                    _unitOfWork.BookingRequest.Context().Entry(farmTmp).CurrentValues.SetValues(farm);
                    result = await _unitOfWork.Farm.UpdateAsync(farmTmp);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Farm());
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

    }
}
