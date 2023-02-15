using System.ComponentModel.DataAnnotations;
using SystemTech.Core.Constant;

namespace SystemTech.Core.ValidationAttributes
{
    public class SortingFieldsInAttribute: ValidationAttribute
    {
        private readonly string[] _fields = new[]
        {
            Criteria.DefaultEarliest,
            Criteria.DefaultLatest,
            Criteria.DefaultRecentlyChange,
            Criteria.DefaultPriority,
        };

        public string[] ExtendFields = null;

        public SortingFieldsInAttribute() { }

        public SortingFieldsInAttribute(params string[] orderFields)
        {
            ExtendFields = orderFields;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            List<string> availableFields = _fields.ToList();
            if (ExtendFields != null)
            {
                availableFields.AddRange(ExtendFields.ToList());
            }
            
            string fieldStr = (string) value;

            bool result = availableFields.Any(fieldName => fieldName == fieldStr);
            
            ErrorMessage = GetErrorMessage(fieldStr, String.Join(", ", _fields.ToArray()));
            
            return result;
        }
        
        private string GetErrorMessage(string fieldName, string fields) =>
            $"This '{fieldName}' field is not yet supported. Supported operators: {fields}.";
    }
}