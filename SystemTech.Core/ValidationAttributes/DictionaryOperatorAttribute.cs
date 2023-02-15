using System.ComponentModel.DataAnnotations;
using SystemTech.Core.Constant;

namespace SystemTech.Core.ValidationAttributes
{
    public class DictionaryOperatorAttribute : ValidationAttribute
    {
        private static readonly List<string> DefaultOperatorList = new List<string>
        {
            Operator.GreaterThan,
            Operator.GreaterThanOrEqual,
            Operator.LessThan,
            Operator.LessThanOrEqual,
            Operator.Equal,
            Operator.Contains,
            Operator.StartsWith
        };
        
        private readonly string _operators = string.Join(",", DefaultOperatorList);

        public DictionaryOperatorAttribute()
        {
        }
        
        public DictionaryOperatorAttribute(params string[] operators)
        {
            _operators = String.Join(",", operators);
        }

        public override bool IsValid(object value)
        {
            List<string> unsupportedOperators = new List<string>();
            
            if (value == null) return true;

            if (value is Dictionary<string, string> dictionary)
            {
                unsupportedOperators = dictionary.Keys.Where(key => !_operators.Contains(key)).ToList();

                if (unsupportedOperators.Count > 0)
                {
                    ErrorMessage = GetErrorMessage(String.Join(", ", unsupportedOperators.ToArray()));
                }
            }
            
            return unsupportedOperators.Count == 0;
        }
        
        private string GetErrorMessage(string operatorName) =>
            $"This '{operatorName}' operator is not yet supported. Supported operators: {_operators}.";
    }
}