using System.ComponentModel.DataAnnotations;
using SystemTech.Core.Constant;
using SystemTech.Core.Messages;
using SystemTech.Core.Utils;
using SystemTech.Core.ValidationAttributes;

namespace StudentSystem.Web.Apis.Controllers.Params
{
    public class StudentsGetParams : QueryParams
    {
        [SortingFieldsIn(ExtendFields = new[] {Criteria.DefaultLatest})]
        public override string Sort { get; set; } = Criteria.DefaultLatest;
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int? IdentityNumber { get; set; }

        protected override string GetField()
        {
            return ReflectionHelpers.GetStringValue(typeof(DefaultCriteriaAlias), Sort);
        }
    }

    public class StudentAddParams
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? IdentityNumber { get; set; } 
    }
    
    public class StudentUpdateParams
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int? IdentityNumber { get; set; }
    }
}