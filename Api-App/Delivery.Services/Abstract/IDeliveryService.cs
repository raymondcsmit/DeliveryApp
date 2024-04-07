using Core;
using DeliveryApp.DAL.Entities;

namespace Delivery.Services.Abstract
{
    public interface IDeliveryService
    {
        Task<ResponseResult> GetUpcoming(int count, int userId);
        Task<ResponseResult> GetPast(int count, int userId);

        Task<ResponseResult> GetAllDeliveriesAsync(int pageNumber, int pageSize);
        Task<ResponseResult> GetDeliveryByIdAsync(int id);
        Task<ResponseResult> AddDeliveryAsync(DeliveryDetails product);
        Task<ResponseResult> UpdateDeliveryAsync(DeliveryDetails product);
        Task<ResponseResult> DeleteDeliveryAsync(int id);
    }

}
