using KoiOrderingSystemInJapan.Common;
using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data;
using KoiOrderingSystemInJapan.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiOrderingSystemInJapan.Data.Request.BookingRequests;
using Azure.Core;

namespace KoiOrderingSystemInJapan.Service
{
    public interface IBookingRequestService
    {
        Task<IBusinessResult> GetAllNoFilter();
        Task<IBusinessResult> GetAll(BookingRequestRequest request, int page, int pageSize);
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetBookingRequestsWithNoSale();
        Task<IBusinessResult> GetById(Guid id);
        Task<IBusinessResult> Save(BookingRequest bookingRequest);
        Task<IBusinessResult> DeleteById(Guid id);

    }
    public class BookingRequestService : IBookingRequestService
    {
        private readonly UnitOfWork _unitOfWork;
        public BookingRequestService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteById(Guid code)
        {
            try
            {
                var bookingRequest = await _unitOfWork.BookingRequest.GetByIdAsync(code);
                if (bookingRequest == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new BookingRequest());
                }
                else
                {
                    var result = await _unitOfWork.BookingRequest.RemoveAsync(bookingRequest);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, bookingRequest);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, bookingRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllNoFilter()
        {
           try
            {
                var result = await _unitOfWork.BookingRequest.GetBookingRequestsWithNoFilter();
                if (result == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<BookingRequest>());
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
            } catch (Exception ex)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll(BookingRequestRequest request, int page, int pageSize)
        {
            var item = await _unitOfWork.BookingRequest.GetAllAsync(request, page, pageSize);
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

        public async Task<IBusinessResult> GetAll()
        {
            var result = await _unitOfWork.BookingRequest.GetAllAsync();
            if (result == null || !result.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, result);
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
        }

        public async Task<IBusinessResult> GetBookingRequestsWithNoSale()
        {
            var bookingRequest = await _unitOfWork.BookingRequest.GetBookingRequestsWithNoSale();
            if (bookingRequest == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<BookingRequest>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, bookingRequest);
            }
        }

        public async Task<IBusinessResult> GetById(Guid code)
        {
            var bookingRequest = await _unitOfWork.BookingRequest.GetByIdAsync(code);
            if (bookingRequest == null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, new BookingRequest());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, bookingRequest);
            }
        }

        public async Task<IBusinessResult> Save(BookingRequest bookingRequest)
        {
            try
            {
                int result = -1;
                var bookingRequestTmp = _unitOfWork.BookingRequest.GetById(bookingRequest.Id);

                if (bookingRequestTmp == null)
                {
                    result = await _unitOfWork.BookingRequest.CreateAsync(bookingRequest);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new BookingRequest());
                    }
                }
                else
                {
                    _unitOfWork.BookingRequest.Context().Entry(bookingRequestTmp).CurrentValues.SetValues(bookingRequest);
                    result = await _unitOfWork.BookingRequest.UpdateAsync(bookingRequestTmp);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, result);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, new BookingRequest());
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
