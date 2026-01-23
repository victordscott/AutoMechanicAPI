using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Helpers
{
    public static class EntityHelper
    {
        public static void SetPropertyValue<T>(T entity, string propertyName, object value) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");

            if (!property.CanWrite)
                throw new InvalidOperationException($"Property '{propertyName}' is read-only.");

            property.SetValue(entity, value);
        }
    }
}
