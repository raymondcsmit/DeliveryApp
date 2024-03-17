using System.Collections.Concurrent;
using System.Reflection;

namespace Core
{
    public static class EntityMetadataCache
    {
        private static readonly ConcurrentDictionary<Type, EntityMetadata> Metadata = new ConcurrentDictionary<Type, EntityMetadata>();

        public static EntityMetadata GetMetadata<T>() where T : class
        {
            var type = typeof(T);
            if (!Metadata.ContainsKey(type))
            {
                Metadata[type] = BuildMetadata<T>();
            }

            return Metadata[type];
        }

        private static EntityMetadata BuildMetadata<T>() where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var keyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);
            var propertyNames = properties.Select(p => p.Name).ToList();

            return new EntityMetadata
            {
                KeyProperty = keyProperty,
                PropertyNames = propertyNames
            };
        }

        public class EntityMetadata
        {
            public PropertyInfo KeyProperty { get; set; }
            public List<string> PropertyNames { get; set; }
        }
    }

}
