using StudentSystem.Web.Apis.Models;
using SystemTech.Core.Messages;

namespace StudentSystem.Web.Apis.Services.Students
{
    public class StudentsGetServiceFields : QueryFields
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int? IdentityNumber { get; set; }
    }
    
    public class StudentGetByIdServiceFields : StudentModel
    {
    }

    public class StudentAddServiceFields : StudentModel
    {
    }

    public class StudentUpdateServiceFields : StudentModel
    {
    }
    
    public class StudentRemoveServiceFields : StudentModel
    {
    }
}