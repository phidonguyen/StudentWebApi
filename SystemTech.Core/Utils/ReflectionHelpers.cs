using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SystemTech.Core.Utils
{
    public static class ReflectionHelpers
    {
        public static string[] auditFields = {"Id", "CreatedDate", "UpdatedDate", "CreatedBy", "UpdatedBy"};
        public static List<PropertyInfo> GetKeyAttributes(Type type)
        {
            return type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(KeyAttribute))).ToList();
        }
        
        public static List<PropertyInfo> GetKeyAttributes<T>()
        {
            return typeof(T).GetProperties().Where(property => Attribute.IsDefined(property, typeof(KeyAttribute))).ToList();
        }

        public static object[] GetKeyValues(object entity)
        {
            List<object> keyValues = new List<object>();

            List<PropertyInfo> keyProperties = entity.GetType().GetProperties().Where(property => Attribute.IsDefined(property, typeof(KeyAttribute))).ToList();
            foreach (PropertyInfo propertyInfo in keyProperties)
            {
                object keyValue = propertyInfo.GetValue(entity, null);
                if (keyValue == null) continue;
                keyValues.Add(keyValue);
            }
            
            return keyValues.ToArray();
        }
        
        public static object MergeFieldsChanged(Type type, object source, object destination)
        {
            // only update not null
            var properties = type.GetProperties().Where(_ => !auditFields.Contains(_.Name));
            foreach (var property in properties)
            {
                var newValue = property.GetValue(source, null);
                var oldValue = property.GetValue(destination, null);
                //hard bypass foreign key
                if (IsChangedValue(oldValue, newValue) || Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                    continue;

                property.SetValue(destination, newValue, null);
            }

            return destination;
        }
        
        public static TEntity MergeFieldsChanged<TInput, TEntity>(TInput input, TEntity source, TEntity destination)
        {
            var properties = typeof(TEntity).GetProperties().Where(_ => !auditFields.Contains(_.Name));
            var inputProperties = typeof(TInput).GetProperties();
            // only update not null
            foreach (var property in properties)
            {
                var newValue = property.GetValue(source, null);
                var oldValue = property.GetValue(destination, null);
                var inputProperty = inputProperties.FirstOrDefault(_ => _.Name == property.Name);
                
                //hard bypass foreign key
                if (inputProperty == null || !IsChangedValue(oldValue, newValue) || Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                    continue;

                property.SetValue(destination, newValue, null);
            }

            return destination;
        }
        
        public static T MergeFieldsChanged<T>(T source, T destination)
        {
            var properties = typeof(T).GetProperties().Where(_ => !auditFields.Contains(_.Name));
            // only update not null
            foreach (var property in properties)
            {
                var newValue = property.GetValue(source, null);
                var oldValue = property.GetValue(destination, null);
                //hard bypass foreign key
                if (!IsChangedValue(oldValue, newValue) || Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                    continue;

                property.SetValue(destination, newValue, null);
            }

            return destination;
        }

        private static bool IsChangedValue(object oldValue, object newValue)
        {
            if (newValue == null
                || newValue == oldValue)
                return false;
            return true;
        }
        
        public static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
        
        public static string GetStringValue(Type type, string fieldName)
        {
            string result = string.Empty;
            Type currentType = type;

            if (string.IsNullOrEmpty(fieldName))
                return result;
            
            while (currentType != null)
            {
                var fieldInfo = currentType.GetField(fieldName);
                if (fieldInfo != null)
                {
                    object fieldObject = fieldInfo.GetValue(null);
                    result = fieldObject?.ToString();
                    break;
                }
                
                // next level
                currentType = currentType.BaseType;
            }

            return result;
        }

        public static bool IsExistConstants<T>(string phrase)
        {
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Where(i => i.IsLiteral);
            return fields.Any(field => field.GetRawConstantValue()?.ToString() == phrase);
        }
    }
}