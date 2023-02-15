using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SystemTech.Core.ValidationAttributes
{
    public class RequiredListTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            
            var listValue = value as IEnumerable;
            
            return listValue != null && listValue.GetEnumerator().MoveNext();
        }
    }
}