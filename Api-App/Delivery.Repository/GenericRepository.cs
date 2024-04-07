using Core;
using Dapper;
using DeliveryApp.DAL;
using Microsoft.Extensions.Options;
using System.Data;

namespace Delivery.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DeliveryContext _context;
        private readonly string _tableName;
        private readonly string _keyColumnName; // Store key column name here
        private readonly EntityMetadataCache.EntityMetadata _metadata;
        private readonly EntityColumnMappingProvider _columnMappingProvider;

        public GenericRepository(DeliveryContext context, EntityColumnMappingProvider columnMappingProvider, IOptions<RepositoryOptions> options)
        {
            _context = context;
            _columnMappingProvider = columnMappingProvider;
            _metadata = EntityMetadataCache.GetMetadata<T>();

            var typeName = typeof(T).Name;
            if (!options.Value.TableNames.TryGetValue(typeName, out _tableName))
            {
                throw new KeyNotFoundException($"No table name found for type {typeName}.");
            }

            _keyColumnName = _columnMappingProvider.GetColumnName(typeName, _metadata.KeyProperty.Name);
        }
        private string GetColumns()
        {
            var properties = typeof(T).GetProperties();
            var columnNames = properties
                .Select(p => new { ColumnName = _columnMappingProvider.GetColumnName(typeof(T).Name, p.Name), PropertyName = p.Name })
                .Where(x => x.ColumnName != null) // Filter out properties for which the column name is null
                .Select(x => $"{x.ColumnName} AS {x.PropertyName}");
            return string.Join(", ", columnNames);
        }

        private string GetColumns_Forced()
        {
            var properties = typeof(T).GetProperties();
            var columnNames = properties.Select(p =>
                $"{_columnMappingProvider.GetColumnName(typeof(T).Name, p.Name)} AS {p.Name}");
            return string.Join(", ", columnNames);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var columns = GetColumns();
            var query = $"SELECT {columns} FROM {_tableName}";
            return await connection.QueryAsync<T>(query);
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            using var connection = _context.CreateConnection();
            var offset = (pageNumber - 1) * pageSize;
            var columns = GetColumns();
            var query = $@"
                SELECT {columns} FROM {_tableName}
                ORDER BY {_keyColumnName}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";

            return await connection.QueryAsync<T>(query, new { Offset = offset, PageSize = pageSize });
        }
        public async Task<IEnumerable<T>> GetByAsync(Dictionary<string, object> filters, IDbTransaction transaction)
        {

            var columns = GetColumns();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var filter in filters)
            {
                var columnName = _columnMappingProvider.GetColumnName(typeof(T).Name, filter.Key);
                // Ensure proper column name mapping and prevent SQL injection
                if (!string.IsNullOrWhiteSpace(columnName) && filter.Value != null)
                {
                    whereClauses.Add($"{columnName} = @{filter.Key}");
                    parameters.Add($"@{filter.Key}", filter.Value);
                }
            }

            var whereClause = whereClauses.Any() ? $"WHERE {string.Join(" AND ", whereClauses)}" : string.Empty;
            var query = $"SELECT {columns} FROM {_tableName} {whereClause}";

            return await transaction.Connection.QueryAsync<T>(query, parameters, transaction);
        }

        public async Task<IEnumerable<T>> GetTop(Dictionary<string, object> filters, string orderByColumn = null, string orderByDirection = "ASC")
        {
            var columns = GetColumns();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var filter in filters)
            {
                var columnName = _columnMappingProvider.GetColumnName(typeof(T).Name, filter.Key);
                // Ensure proper column name mapping and prevent SQL injection
                if (!string.IsNullOrWhiteSpace(columnName) && filter.Value != null)
                {
                    whereClauses.Add($"{columnName} = @{filter.Key}");
                    parameters.Add($"@{filter.Key}", filter.Value);
                }
            }

            var whereClause = whereClauses.Any() ? $"WHERE {string.Join(" AND ", whereClauses)}" : string.Empty;

            // Initialize the orderByClause to an empty string
            var orderByClause = string.Empty;
            if (!string.IsNullOrWhiteSpace(orderByColumn))
            {
                var orderColumn = _columnMappingProvider.GetColumnName(typeof(T).Name, orderByColumn);
                // Ensure the column name for ordering is valid to prevent SQL injection
                if (!string.IsNullOrWhiteSpace(orderColumn))
                {
                    orderByClause = $"ORDER BY {orderColumn} {orderByDirection}";
                }
            }

            // Modify the query to include ORDER BY and limit the results to top 4
            // This is for SQL Server, adjust the LIMIT clause based on your database (e.g., "LIMIT 4" for MySQL or PostgreSQL)
            var query = $"SELECT TOP 4 {columns} FROM {_tableName} {whereClause} {orderByClause}";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<T>(query, parameters);
        }



        public async Task<IEnumerable<T>> GetByAsync(Dictionary<string, object> filters)
        {
            using var connection = _context.CreateConnection();
            var columns = GetColumns();
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var filter in filters)
            {
                var columnName = _columnMappingProvider.GetColumnName(typeof(T).Name, filter.Key);
                // Ensure proper column name mapping and prevent SQL injection
                if (!string.IsNullOrWhiteSpace(columnName) && filter.Value != null)
                {
                    whereClauses.Add($"{columnName} = @{filter.Key}");
                    parameters.Add($"@{filter.Key}", filter.Value);
                }
            }

            var whereClause = whereClauses.Any() ? $"WHERE {string.Join(" AND ", whereClauses)}" : string.Empty;
            var query = $"SELECT {columns} FROM {_tableName} {whereClause}";

            return await connection.QueryAsync<T>(query, parameters);
        }
        public async Task<IEnumerable<TFirst>> ExecuteQueryMultiMappingAsync<TFirst, TSecond, TKey>(
     string query,
     Func<TFirst, TSecond, TKey, TFirst> map,
     object parameters,
     string splitOn,
     string keyPropertyName)
        {
            using var connection = _context.CreateConnection();
            var resultDictionary = new Dictionary<TKey, TFirst>();

            await connection.QueryAsync<TFirst, TSecond, TFirst>(
                query,
                (first, second) =>
                {
                    // Use reflection to get the key property
                    var keyProperty = typeof(TFirst).GetProperty(keyPropertyName);
                    if (keyProperty == null)
                        throw new InvalidOperationException($"Property {keyPropertyName} not found on type {typeof(TFirst).Name}.");

                    var key = (TKey)keyProperty.GetValue(first);
                    if (!resultDictionary.TryGetValue(key, out var firstEntry))
                    {
                        firstEntry = first;
                        resultDictionary.Add(key, firstEntry);
                    }

                    map(firstEntry, second, key);
                    return firstEntry;
                },
                parameters,
                splitOn: splitOn
            );

            return resultDictionary.Values;
        }


        public async Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string query, object parameters = null)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<TResult>(query, parameters);
        }
        public async Task<T> GetByIdAsync(object id)
        {
            using var connection = _context.CreateConnection();
            var columns = GetColumns();
            return await connection.QuerySingleOrDefaultAsync<T>($"SELECT {columns} FROM {_tableName} WHERE {_keyColumnName} = @Id", new { Id = id });
        }

        public async Task AddAsync(T entity, IDbTransaction transaction = null)
        {
            var insertQuery = GenerateInsertQuery();
            using var connection = transaction?.Connection ?? _context.CreateConnection();
            await connection.ExecuteAsync(insertQuery, entity, transaction);
        }
        public async Task<int> InsertAsync(T entity, IDbTransaction transaction)
        {
            var insertQuery = GenerateInsertQuery();
            int id = await transaction.Connection.ExecuteScalarAsync<int>(insertQuery, entity, transaction);
            return id;
        }
        //ExecuteScalarAsync<int>
        // New method to add multiple entities within a transaction
        public async Task AddRangeAsync(IEnumerable<T> entities, IDbTransaction transaction = null)
        {
            var insertQuery = GenerateInsertQuery();
            using var connection = transaction?.Connection ?? _context.CreateConnection();
            await connection.ExecuteAsync(insertQuery, entities, transaction);
        }
        public async Task InsertRangeAsync(IEnumerable<T> entities, IDbTransaction transaction)
        {
            var insertQuery = GenerateInsertQuery();
            await transaction.Connection.ExecuteAsync(insertQuery, entities, transaction);
        }
        // Update other methods similarly to include an optional IDbTransaction parameter
        public async Task UpdateAsync(T entity, IDbTransaction transaction = null)
        {
            var updateQuery = GenerateUpdateQuery();
            using var connection = transaction?.Connection ?? _context.CreateConnection();
            await connection.ExecuteAsync(updateQuery, entity, transaction);
        }

        public async Task DeleteAsync(object id, IDbTransaction transaction = null)
        {
            using var connection = transaction?.Connection ?? _context.CreateConnection();
            await connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE {_keyColumnName} = @Id", new { Id = id }, transaction);
        }
        private string GenerateInsertQuery()
        {
            // Filter out properties marked with the [Key] attribute and where column name does not exist
            var properties = typeof(T).GetProperties()
                                      .Where(p => p.CustomAttributes.All(a => a.AttributeType != typeof(KeyAttribute)))
                                      .Where(p => _columnMappingProvider.GetColumnName(typeof(T).Name, p.Name) != null);

            // Generate the list of column names based on the filtered properties
            var columnNames = properties.Select(p => _columnMappingProvider.GetColumnName(typeof(T).Name, p.Name));

            // Join the column names into a single string for the SQL statement
            var columns = string.Join(", ", columnNames);

            // Generate the list of parameter placeholders for the values to be inserted
            var values = string.Join(", ", properties.Select(p => "@" + p.Name));

            // Construct and return the full INSERT INTO SQL statement
            return $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT CAST(SCOPE_IDENTITY() as int);";
        }

        private string GenerateInsertQuery_Forced()
        {
            var properties = typeof(T).GetProperties().Where(p => p.CustomAttributes.All(a => a.AttributeType != typeof(KeyAttribute)));
            var columnNames = properties.Select(p => _columnMappingProvider.GetColumnName(typeof(T).Name, p.Name));
            var columns = string.Join(", ", columnNames);
            var values = string.Join(", ", properties.Select(p => "@" + p.Name));
            return $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";
        }

        private string GenerateUpdateQuery()
        {
            var properties = typeof(T).GetProperties().Where(p => p.CustomAttributes.All(a => a.AttributeType != typeof(KeyAttribute)));
            var setClause = string.Join(", ", properties.Select(p => $"{_columnMappingProvider.GetColumnName(typeof(T).Name, p.Name)} = @{p.Name}"));
            return $"UPDATE {_tableName} SET {setClause} WHERE {_keyColumnName} = @{_metadata.KeyProperty.Name}";
        }
    }
}
