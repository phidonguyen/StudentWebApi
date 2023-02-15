using SystemTech.Core.Messages;

namespace StudentSystem.Web.Apis.Services.Students
{
    public class StudentsGetServiceRequest : BaseRequest<StudentsGetServiceFields>
    {
        public StudentsGetServiceRequest(StudentsGetServiceFields studentsGetServiceFields) : base(studentsGetServiceFields) { }
    }
    
    public class StudentGetByIdServiceRequest : BaseRequest<StudentGetByIdServiceFields>
    {
        public StudentGetByIdServiceRequest(StudentGetByIdServiceFields studentGetByIdServiceFields) : base(studentGetByIdServiceFields) { }
    }
    
    public class StudentAddServiceRequest : BaseRequest<StudentAddServiceFields>
    {
        public StudentAddServiceRequest(StudentAddServiceFields studentAddServiceFields) : base(studentAddServiceFields) { }
    }
    
    public class StudentUpdateServiceRequest : BaseRequest<StudentUpdateServiceFields>
    {
        public StudentUpdateServiceRequest(StudentUpdateServiceFields studentUpdateServiceFields) : base(studentUpdateServiceFields) { }
    }
    
    public class StudentRemoveServiceRequest : BaseRequest<StudentRemoveServiceFields>
    {
        public StudentRemoveServiceRequest(StudentRemoveServiceFields studentRemoveServiceFields) : base(studentRemoveServiceFields) { }
    }
}
