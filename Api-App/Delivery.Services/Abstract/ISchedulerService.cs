using Core;
using Delivery.ViewModel;
using DeliveryApp.DAL.Entities;

namespace Delivery.Services.Abstract
{
    public interface ISchedulerService
    {
        Task<ResponseResult> GetAllAsync(int pageNumber, int pageSize);
        Task<ResponseResult> GetByIdAsync(int id);
        Task<ResponseResult> Subscribe(SubScribeVM subscribe, int userId);
        Task<ResponseResult> AddAsync(Scheduler schedule);
        Task<ResponseResult> UpdateAsync(Scheduler schedule);
        Task<ResponseResult> DeleteAsync(int id);
    }

}
