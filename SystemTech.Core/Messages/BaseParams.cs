using SystemTech.Core.Constant;
using SystemTech.Core.Utils;
using SystemTech.Core.ValidationAttributes;

namespace SystemTech.Core.Messages
{
    public class QueryParams
    {
        [IsUnsignedInteger] public virtual int Page { get; set; } = 0;

        [IsPositiveNumber] public virtual int PageSize { get; set; } = GeneralConst.DefaultRowLimit;

        [SortingFieldsIn]
        public virtual string Sort { get; set; } = Criteria.FieldNameDef;
        
        public QueryParams()
        {
        }

        protected virtual string GetField()
        {
            return ReflectionHelpers.GetStringValue(typeof(DefaultCriteriaAlias), Sort);
        }

        public string GetOrder()
        {
            string orderBy = GetField();
            return string.IsNullOrEmpty(orderBy) ? DefaultCriteriaAlias.Latest : orderBy;
        }
    }
    
    public class CommandParams
    {
    }
}
