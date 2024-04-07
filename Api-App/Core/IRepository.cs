using System.Data;

namespace Core
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetByAsync(Dictionary<string, object> filters);
        Task<IEnumerable<T>> GetByAsync(Dictionary<string, object> filters, IDbTransaction transaction);
        Task<IEnumerable<T>> GetTop(Dictionary<string, object> filters, string orderByColumn = null, string orderByDirection = "ASC");
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string query, object parameters = null);
        Task<IEnumerable<TFirst>> ExecuteQueryMultiMappingAsync<TFirst, TSecond, TKey>(
            string query,
            Func<TFirst, TSecond, TKey, TFirst> map,
            object parameters,
            string splitOn,
            string keyPropertyName);
        Task<T> GetByIdAsync(object id);
        Task AddAsync(T entity, IDbTransaction transaction = null);
        Task<int> InsertAsync(T entity, IDbTransaction transaction);
        Task AddRangeAsync(IEnumerable<T> entities, IDbTransaction transaction = null);
        Task InsertRangeAsync(IEnumerable<T> entities, IDbTransaction transaction);
        Task UpdateAsync(T entity, IDbTransaction transaction = null);
        Task DeleteAsync(object id, IDbTransaction transaction = null);
    }

}
