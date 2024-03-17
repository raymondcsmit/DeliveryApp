using Core;

namespace Delivery.Services.Abstract
{
    public interface ILookupService<T>
    {
        Task<ResponseResult> GetAllAsync(int pageNumber, int pageSize);
        Task<ResponseResult> GetByAsync(Dictionary<string, object> filters);
        Task<ResponseResult> GetAllAsync();
    }



}
