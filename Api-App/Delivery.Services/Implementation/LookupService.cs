using Core;
using Delivery.Services.Abstract;

namespace Delivery.Services.Implementation
{
    public class LookupService<T> : ILookupService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public LookupService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseResult> GetAllAsync(int pageNumber, int pageSize)
        {
            var items = await _repository.GetAllAsync(pageNumber, pageSize);
            if (items == null || !items.Any())
            {
                return ResponseFactory.CreateNotFound($"No {typeof(T).Name} records found.");
            }

            return ResponseFactory.CreateSuccess($"{typeof(T).Name} records retrieved successfully.", items);
        }
        public async Task<ResponseResult> GetByAsync(Dictionary<string, object> filters)
        {
            var items = await _repository.GetByAsync(filters);
            if (items == null || !items.Any())
            {
                return ResponseFactory.CreateNotFound($"No {typeof(T).Name} records found.");
            }

            return ResponseFactory.CreateSuccess($"{typeof(T).Name} records retrieved successfully.", items);
        }
        public async Task<ResponseResult> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (items == null || !items.Any())
            {
                return ResponseFactory.CreateNotFound($"No {typeof(T).Name} records found.");
            }

            return ResponseFactory.CreateSuccess($"{typeof(T).Name} records retrieved successfully.", items);
        }
    }


}
