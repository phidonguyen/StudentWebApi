using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Base.Services;
using StudentSystem.Web.Common.Messages;
using SystemTech.Core.Exceptions;
using SystemTech.Core.Messages;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.Apis.Services.Students
{
    public class StudentService : BaseService, IStudentService
    {
        private readonly Merger _merger;
        private readonly IMapper _mapper;

        public StudentService(
            IHttpContextAccessor httpContextAccessor,
            StudentSystemDbContextFactory dbContextFactory,
            IMapper mapper) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
            _merger = new Merger(mapper.ConfigurationProvider);
        }

        public async Task<StudentsGetServiceResponse> Get(StudentsGetServiceRequest studentsGetServiceRequest)
        {
            StudentsGetServiceResponse studentsGetServiceResponse =
                new StudentsGetServiceResponse(studentsGetServiceRequest);
            try
            {
                BuildFilterStudents(studentsGetServiceRequest, out var condition);

                var studentsQueryable = DbContext.Students.Where(condition.Compile()).AsQueryable();

                _merger.MergeData(studentsGetServiceResponse, studentsQueryable);
            }
            catch (Exception e)
            {
                studentsGetServiceResponse.AddException(e);
            }

            return studentsGetServiceResponse;
        }

        public async Task<StudentGetByIdServiceResponse> GetById(
            StudentGetByIdServiceRequest studentGetByIdServiceRequest)
        {
            StudentGetByIdServiceResponse studentGetByIdServiceResponse =
                new StudentGetByIdServiceResponse(studentGetByIdServiceRequest);
            try
            {
                StudentGetByIdServiceFields studentGetByIdServiceFields = studentGetByIdServiceRequest.Fields;

                var studentDb =
                    await DbContext.Students.FirstOrDefaultAsync(_ => _.Id == studentGetByIdServiceFields.Id);

                if (studentDb == null)
                {
                    var message = $"Student [{studentGetByIdServiceFields.Id}] is not found.";
                    studentGetByIdServiceResponse.AddException(
                        new RecordNotFoundException(message));
                    return studentGetByIdServiceResponse;
                }

                _merger.MergeData(studentGetByIdServiceResponse, studentDb);
            }
            catch (Exception e)
            {
                studentGetByIdServiceResponse.AddException(e);
            }

            return studentGetByIdServiceResponse;
        }

        public async Task<StudentAddServiceResponse> Add(StudentAddServiceRequest studentAddServiceRequest)
        {
            StudentAddServiceResponse studentAddServiceResponse =
                new StudentAddServiceResponse(studentAddServiceRequest);
            try
            {
                StudentAddServiceFields studentAddServiceFields = studentAddServiceRequest.Fields;

                Student student = _merger.Map<Student>(studentAddServiceFields);
                student.UpdatedBy = CurrentUserId;

                //execute data
                var studentEntry = await DbContext.AddAsync(student);

                await DbContext.SaveChangesAsync();

                // merging
                _merger.MergeData(studentAddServiceResponse, studentEntry.Entity);
            }
            catch (Exception e)
            {
                studentAddServiceResponse.AddException(e);
            }

            return studentAddServiceResponse;
        }


        public async Task<StudentUpdateServiceResponse> Update(StudentUpdateServiceRequest studentUpdateServiceRequest)
        {
            StudentUpdateServiceResponse studentUpdateServiceResponse =
                new StudentUpdateServiceResponse(studentUpdateServiceRequest);
            try
            {
                StudentUpdateServiceFields studentUpdateServiceFields = studentUpdateServiceRequest.Fields;

                Student student = _mapper.Map<Student>(studentUpdateServiceFields);
                student.UpdatedBy = CurrentUserId;

                var studentDb = await DbContext.Students.FirstOrDefaultAsync(_ => _.Id == student.Id);
                if (studentDb == null)
                {
                    var message = $"Student [{studentUpdateServiceFields.Id}] is not found.";
                    studentUpdateServiceResponse.AddException(new RecordNotFoundException(message));
                    return studentUpdateServiceResponse;
                }

                ReflectionHelpers.MergeFieldsChanged(studentUpdateServiceFields, student, studentDb);

                var studentEntry = DbContext.Update(studentDb);

                await DbContext.SaveChangesAsync();

                // merging
                _merger.MergeData(studentUpdateServiceResponse, studentEntry.Entity);
            }
            catch (Exception e)
            {
                studentUpdateServiceResponse.AddException(e);
            }

            return studentUpdateServiceResponse;
        }

        public async Task<StudentRemoveServiceResponse> Remove(
            StudentRemoveServiceRequest studentRemoveServiceRequest)
        {
            StudentRemoveServiceResponse studentRemoveServiceResponse =
                new StudentRemoveServiceResponse(studentRemoveServiceRequest);
            try
            {
                StudentRemoveServiceFields studentRemoveServiceFields = studentRemoveServiceRequest.Fields;

                var studentDb =
                    await DbContext.Students.FirstOrDefaultAsync(_ => _.Id == studentRemoveServiceFields.Id);

                if (studentDb == null)
                {
                    var message = $"Student [{studentRemoveServiceFields.Id}] is not found.";
                    studentRemoveServiceResponse.AddException(new RecordNotFoundException(message));
                    return studentRemoveServiceResponse;
                }

                DbContext.Remove(studentDb);

                await DbContext.SaveChangesAsync();

                _merger.MergeData(studentRemoveServiceResponse, studentDb);
            }
            catch (Exception e)
            {
                studentRemoveServiceResponse.AddException(e);
            }

            return studentRemoveServiceResponse;
        }

        #region Helper

        private void BuildFilterStudents(StudentsGetServiceRequest studentsGetServiceRequest,
            out Expression<Func<Student, bool>> condition)
        {
            condition = _ => true;

            StudentsGetServiceFields studentsGetServiceFields = studentsGetServiceRequest.Fields;

            if (!string.IsNullOrEmpty(studentsGetServiceFields.Name))
            {
                var compiled = condition.Compile();

                condition = _ =>
                    compiled(_) && _.Name.ToLower().Trim().Contains(studentsGetServiceFields.Name.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(studentsGetServiceFields.PhoneNumber))
            {
                var compiled = condition.Compile();

                condition = _ => compiled(_) && _.PhoneNumber
                    .Contains(studentsGetServiceFields.PhoneNumber);
            }

            if (studentsGetServiceFields.IdentityNumber > 0)
            {
                var compiled = condition.Compile();

                condition = _ => compiled(_) && _.IdentityNumber == studentsGetServiceFields.IdentityNumber;
            }
        }

        #endregion
    }
}