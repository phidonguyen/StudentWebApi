using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Apis.Services.Students;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Configurations;
using SystemTech.Core.Messages;
using Xunit;

namespace StudentSystem.Web.Test.ServicesTest.Students
{
    public class StudentServiceTest
    {
        private readonly StudentService _studentService;

        #region Constuctor

        public StudentServiceTest()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var objectMappingMock = new Mock<ObjectMapping>();
            var modelMappingMock = new Mock<ModelMapping>();
            var configurationProvider = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(objectMappingMock.Object);
                    mc.AddProfile(modelMappingMock.Object);
                })
                .CreateMapper().ConfigurationProvider;
            var mergerMock = new Mock<Merger>(configurationProvider);
            var dbContextFactoryMock = new Mock<IDbContextFactory<StudentSystemDbContext>>();
            dbContextFactoryMock.Setup(f => f.CreateDbContext())
                .Returns(new StudentSystemDbContext(new DbContextOptionsBuilder<StudentSystemDbContext>()
                    .UseInMemoryDatabase("InMemoryTest")
                    .Options));
            var studentSystemDbContextFactoryMock = new Mock<StudentSystemDbContextFactory>(dbContextFactoryMock.Object);
            _studentService = new StudentService(httpContextAccessorMock.Object,
                studentSystemDbContextFactoryMock.Object, mergerMock.Object);
        }

        #endregion
        
        [Fact]
        public void AddStudent_DataIsNull_ThrownException()
        {
            // Arrange
            StudentAddServiceRequest studentAddServiceResponse = null;

            // Act & Assert                    
            Assert.ThrowsAsync<Exception>(() => _studentService.Add(studentAddServiceResponse));
        }

        [Fact]
        public async Task AddStudent_DataIsValid_ReturnDataResponse()
        {
            // Arrange
            StudentAddServiceRequest studentAddServiceRequest = new StudentAddServiceRequest(new StudentAddServiceFields
            {
                Name = "Student Test",
                PhoneNumber = "123456789",
                IdentityNumber = 123213
            });

            // Action
            StudentAddServiceResponse response = await _studentService.Add(studentAddServiceRequest);

            // Assert
            Assert.NotEmpty(response.Result.Id);
        }

        [Fact]
        public async Task GetDetailStudent_StudentIdDoesntExist_ReturnErrorMessage()
        {
            // Arrange
            StudentGetByIdServiceRequest studentGetByIdServiceRequest = new StudentGetByIdServiceRequest(
                new StudentGetByIdServiceFields
                {
                    Id = "123"
                });

            // Action
            StudentGetByIdServiceResponse response = await _studentService.GetById(studentGetByIdServiceRequest);

            // Assert
            Assert.NotEmpty(response.ErrorMessages);
        }
    }
}