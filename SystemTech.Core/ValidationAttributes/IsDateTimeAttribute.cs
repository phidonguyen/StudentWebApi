using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SystemTech.Core.ValidationAttributes
{
    public class IsDateTimeAttribute : ValidationAttribute
    {
        private string _format = "yyyy-MM-dd";

        public IsDateTimeAttribute() { }

        public IsDateTimeAttribute(string format)
        {
            _format = format;
        }

        public override bool IsValid(object value)
        {
            return value == null || IsValid(value.ToString());
        }

        private bool IsValid(string value)
        {
            if (DateTime.TryParseExact(value, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return true;
            
            ErrorMessage = GetErrorMessage(value);

            return false;
        }

        private string GetErrorMessage(string value) =>
                 $"The value: '{value}' is not correct data type. The value should be '{_format}' data type.";
    }
}