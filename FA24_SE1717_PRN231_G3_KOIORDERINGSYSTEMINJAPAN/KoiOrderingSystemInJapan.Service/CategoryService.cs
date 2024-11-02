using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiOrderingSystemInJapan.Service
{
    public interface ICategoryService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Save(Category bookingRequest);
        Task<IBusinessResult> DeleteById(Guid id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> GetAll()
        {
            #region Business rule

            #endregion
            var bookingRequest = await _unitOfWork.Category.GetAllAsync();
            if (bookingRequest == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, bookingRequest);
            }
        }

        public async Task<IBusinessResult> GetById(Guid code)
        {
            var bookingRequest = await _unitOfWork.Category.GetByIdAsync(code);
            if (bookingRequest == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new Category());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, bookingRequest);
            }
        }

        public async Task<IBusinessResult> Save(Category bookingRequest)
        {
            try
            {
                int result = -1;
                var bookingRequestTmp = _unitOfWork.Category.GetById(bookingRequest.Id);

                if (bookingRequestTmp == null)
                {
                    result = await _unitOfWork.Category.CreateAsync(bookingRequest);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Category());
                    }
                }
                else
                {
                    _unitOfWork.Category.Context().Entry(bookingRequestTmp).CurrentValues.SetValues(bookingRequest);
                    result = await _unitOfWork.Category.UpdateAsync(bookingRequestTmp);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new Category());
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public async Task<IBusinessResult> DeleteById(Guid id)
        {
            try
            {
                var cate = await _unitOfWork.Category.GetByIdAsync(id);
                if (cate != null)
                {
                    var result = await _unitOfWork.Category.RemoveAsync(cate);
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


    }
}
