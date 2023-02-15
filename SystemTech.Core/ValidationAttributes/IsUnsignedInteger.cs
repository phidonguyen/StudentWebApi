using System.ComponentModel.DataAnnotations;

namespace SystemTech.Core.ValidationAttributes
{
    public class IsUnsignedInteger : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is int page && page >= 0;
        }
    }
    
    public class IsPositiveNumber : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is int page && page > 0;
        }
    }
    
    public class ContainsAttribute : ValidationAttribute
    {
        private readonly IEnumerable<string> _values;

        public ContainsAttribute(string[] values)
        {
            _values = values;
        }

        public ContainsAttribute(int[] values)
        {
            _values = values.Select(x => x.ToString());
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (_values != null && !_values.Contains(value))
            {
                var valuesString = string.Join(", ", _values.Select(x => $"'{x}'"));
                var message = $"Provided value for {validationContext.MemberName} property is not valid. Valid values are {valuesString}.";
                return new ValidationResult(message);
            }

            return ValidationResult.Success;
        }
    }
}
