using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.Web.Apis.Controllers.Params;
using StudentSystem.Web.Apis.Services.Students;
using StudentSystem.Web.Base.Controllers;

namespace StudentSystem.Web.Apis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:ApiVersion}/student")]
    [ApiVersion("1.0")]
    public class StudentController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;

        public StudentController(IMapper mapper, IStudentService studentService)
        {
            _mapper = mapper;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] StudentsGetParams studentsGetParams)
        {
            StudentsGetServiceFields studentsGetServiceFields =
                _mapper.Map<StudentsGetServiceFields>(studentsGetParams);
            StudentsGetServiceRequest studentsGetServiceRequest =
                new StudentsGetServiceRequest(studentsGetServiceFields);
            StudentsGetServiceResponse studentsGetServiceResponse =
                await _studentService.Get(studentsGetServiceRequest);

            return ResponseCollection(studentsGetServiceResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            StudentGetByIdServiceFields studentGetByIdServiceFields = new StudentGetByIdServiceFields
            {
                Id = id
            };
            StudentGetByIdServiceRequest studentGetByIdServiceRequest =
                new StudentGetByIdServiceRequest(studentGetByIdServiceFields);
            StudentGetByIdServiceResponse studentGetByIdServiceResponse =
                await _studentService.GetById(studentGetByIdServiceRequest);

            return ResponseItem(studentGetByIdServiceResponse);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] StudentAddParams studentAddParams)
        {
            StudentAddServiceFields studentAddServiceFields = _mapper.Map<StudentAddServiceFields>(studentAddParams);
            StudentAddServiceRequest studentAddServiceRequest = new StudentAddServiceRequest(studentAddServiceFields);
            StudentAddServiceResponse studentAddServiceResponse = await _studentService.Add(studentAddServiceRequest);

            return ResponseItem(studentAddServiceResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] StudentUpdateParams studentUpdateParams)
        {
            StudentUpdateServiceFields studentUpdateServiceFields =
                _mapper.Map<StudentUpdateServiceFields>(studentUpdateParams);
            studentUpdateServiceFields.Id = id;
            StudentUpdateServiceRequest studentUpdateServiceRequest =
                new StudentUpdateServiceRequest(studentUpdateServiceFields);
            StudentUpdateServiceResponse studentUpdateServiceResponse =
                await _studentService.Update(studentUpdateServiceRequest);

            return ResponseItem(studentUpdateServiceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            StudentRemoveServiceFields studentRemoveServiceFields =
                new StudentRemoveServiceFields
                {
                    Id = id
                };

            StudentRemoveServiceRequest studentRemoveServiceRequest =
                new StudentRemoveServiceRequest(studentRemoveServiceFields);
            StudentRemoveServiceResponse studentRemoveServiceResponse =
                await _studentService.Remove(studentRemoveServiceRequest);

            return ResponseItem(studentRemoveServiceResponse);
        }
    }
}