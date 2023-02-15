using System.ComponentModel.DataAnnotations;

namespace SystemTech.Core.ValidationAttributes
{
    public class DictionaryDataTypeAttribute : ValidationAttribute
    {
        private readonly Type _type;
        public DictionaryDataTypeAttribute(Type type)
        {
            _type = type;
        }

        public override bool IsValid(object value)
        {
            List<string> failedValues = new List<string>();
            
            if (value == null) return true;
            
            if (value is Dictionary<string, string> dictionary)
            {
                failedValues = dictionary.Values.Where(valuePerKey => !TryParse(valuePerKey, _type)).ToList();

                if (failedValues.Count > 0)
                {
                    ErrorMessage = GetErrorMessage(String.Join(", ", failedValues.ToArray()));
                }
            }
            
            return failedValues.Count == 0;
        }
        
        private string GetErrorMessage(string values) =>
            $"The value: '{values}' is not correct data type. The value should be '${_type.Name}' data type.";
        
        private bool TryParse(string value, Type type)
        {
            bool result = false;
            
            try
            {
                if (type == typeof(bool))
                {
                    result = bool.TryParse(value, out _);
                }
                else if (type == typeof(byte))
                {
                    result = byte.TryParse(value, out _);
                }
                else if (type == typeof(char))
                {
                    result = char.TryParse(value, out _);
                }
                else if (type == typeof(decimal))
                {
                    result = decimal.TryParse(value, out _);
                }
                else if (type == typeof(double))
                {
                    result = double.TryParse(value, out _);
                }
                else if (type == typeof(float))
                {
                    result = float.TryParse(value, out _);
                }
                else if (type == typeof(int))
                {
                    result = int.TryParse(value, out _);
                }
                else if (type == typeof(long))
                {
                    result = long.TryParse(value, out _);
                }
                else if (type == typeof(sbyte))
                {
                    result = sbyte.TryParse(value, out _);
                }
                else if (type == typeof(short))
                {
                    result = short.TryParse(value, out _);
                }
                else if (type == typeof(uint))
                {
                    result = uint.TryParse(value, out _);
                }
                else if (type == typeof(ulong))
                {
                    result = ulong.TryParse(value, out _);
                }
                else if (type == typeof(ushort))
                {
                    result = ushort.TryParse(value, out _);
                }
                else
                {
                    throw new NotSupportedException($"DataType {type} is not supported.");
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}