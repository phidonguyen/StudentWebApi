using StudentSystem.Web.Apis.Models;
using SystemTech.Core.Messages;

namespace StudentSystem.Web.Apis.Services.Students
{
    public class StudentsGetServiceResponse : BaseResponse<List<StudentModel>, StudentsGetServiceFields>
    {
        public StudentsGetServiceResponse(StudentsGetServiceRequest studentsGetServiceRequest) : base(studentsGetServiceRequest) { }
    }
    
    public class StudentGetByIdServiceResponse : BaseResponse<StudentModel, StudentGetByIdServiceFields>
    {
        public StudentGetByIdServiceResponse(StudentGetByIdServiceRequest studentGetByIdServiceRequest) : base(studentGetByIdServiceRequest) { }
    }
    
    public class StudentAddServiceResponse : BaseResponse<StudentModel, StudentAddServiceFields>
    {
        public StudentAddServiceResponse(StudentAddServiceRequest studentAddServiceRequest) : base(studentAddServiceRequest) { }
    }
    
    public class StudentUpdateServiceResponse : BaseResponse<StudentModel, StudentUpdateServiceFields>
    {
        public StudentUpdateServiceResponse(StudentUpdateServiceRequest studentUpdateServiceRequest) : base(studentUpdateServiceRequest) { }
    }
    
    public class StudentRemoveServiceResponse : BaseResponse<StudentModel, StudentRemoveServiceFields>
    {
        public StudentRemoveServiceResponse(StudentRemoveServiceRequest studentRemoveServiceRequest) : base(studentRemoveServiceRequest) { }
    }
}
