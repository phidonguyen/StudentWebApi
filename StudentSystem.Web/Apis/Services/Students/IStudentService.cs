namespace StudentSystem.Web.Apis.Services.Students
{
    public interface IStudentService
    {
        Task<StudentsGetServiceResponse> Get(StudentsGetServiceRequest studentsGetServiceRequest);
        Task<StudentGetByIdServiceResponse> GetById(StudentGetByIdServiceRequest studentGetByIdServiceRequest);
        Task<StudentAddServiceResponse> Add(StudentAddServiceRequest studentAddServiceRequest);
        Task<StudentUpdateServiceResponse> Update(StudentUpdateServiceRequest studentUpdateServiceRequest);
        Task<StudentRemoveServiceResponse> Remove(StudentRemoveServiceRequest studentRemoveServiceRequest);
    }
}
