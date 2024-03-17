using Microsoft.Extensions.Configuration;

namespace Delivery.Repository
{
    public class EntityColumnMappingProvider
    {
        private readonly IConfigurationSection _entityColumnMappingsSection;

        public EntityColumnMappingProvider(IConfigurationSection entityColumnMappingsSection)
        {
            _entityColumnMappingsSection = entityColumnMappingsSection;
        }
        public string GetColumnName(string entityName, string propertyName)
        {
            // Retrieve the specific entity mapping section
            var entityMappingSection = _entityColumnMappingsSection.GetSection(entityName);
            if (entityMappingSection.Exists())
            {
                var columnName = entityMappingSection[propertyName];
                return columnName != null ? columnName : null;
            }
            return null;
        }
        public string GetColumnName_Forced(string entityName, string propertyName)
        {
            // Retrieve the specific entity mapping section
            var entityMappingSection = _entityColumnMappingsSection.GetSection(entityName);
            if (entityMappingSection.Exists())
            {
                // Try to get the column name for the property; if not found, return the property name
                return entityMappingSection[propertyName] ?? propertyName;
            }
            return propertyName;
        }
    }

    //public class EntityColumnMappingProvider
    //{
    //    private readonly Dictionary<string, Dictionary<string, string>> _mappings;

    //    public EntityColumnMappingProvider(IConfiguration configuration)
    //    {
    //        // Load and parse the mappings from configuration
    //        _mappings = configuration.GetSection("EntityColumnMappings")
    //            .Get<Dictionary<string, Dictionary<string, string>>>();
    //    }

    //    public string GetColumnName<T>(string propertyName)
    //    {
    //        var entityName = typeof(T).Name;
    //        if (_mappings.TryGetValue(entityName, out var propertyMappings) &&
    //            propertyMappings.TryGetValue(propertyName, out var columnName))
    //        {
    //            return columnName;
    //        }
    //        return propertyName; // Fallback to the property name if mapping is not found
    //    }
    //}

}
