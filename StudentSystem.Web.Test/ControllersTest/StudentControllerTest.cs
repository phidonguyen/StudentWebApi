using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Apis.Controllers;
using StudentSystem.Web.Apis.Controllers.Params;
using StudentSystem.Web.Apis.Services.Students;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Configurations;
using SystemTech.Core.Messages;
using Xunit;
using Moq;

namespace StudentSystem.Web.Test.ControllersTest
{
    public class StudentControllerTest
    {
        #region Setup

        private readonly StudentController _studentController;

        public StudentControllerTest()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var objectMappingMock = new Mock<ObjectMapping>();
            var modelMappingMock = new Mock<ModelMapping>();
            var configurationProvider = new MapperConfiguration(mc =>
            {
                mc.AddProfile(objectMappingMock.Object);
                mc.AddProfile(modelMappingMock.Object);
            }).CreateMapper().ConfigurationProvider;
            var mergerMock = new Mock<Merger>(configurationProvider);
            var dbContextFactoryMock = new Mock<IDbContextFactory<StudentSystemDbContext>>();
            dbContextFactoryMock.Setup(f => f.CreateDbContext())
                .Returns(new StudentSystemDbContext(new DbContextOptionsBuilder<StudentSystemDbContext>()
                    .UseInMemoryDatabase("InMemoryTest")
                    .Options));
            var studentSystemDbContextFactoryMock =
                new Mock<StudentSystemDbContextFactory>(dbContextFactoryMock.Object);
            var studentServiceMock = new Mock<StudentService>(httpContextAccessorMock.Object,
                studentSystemDbContextFactoryMock.Object, mergerMock.Object);
            _studentController = new StudentController(mergerMock.Object, studentServiceMock.Object);
        }

        #endregion

        [Fact]
        public async Task GetDetailStudent_DataDoesntExist_ReturnStatusCode404()
        {
            // Action
            var response = await _studentController.GetById("123") as NotFoundObjectResult;

            //Assert
            Assert.Equal(404, response!.StatusCode);
        }

        [Fact]
        public async Task AddStudent_DataIsValid_ReturnStatusCode200()
        {
            // Arrange
            StudentAddParams studentAddParams = new StudentAddParams
            {
                Name = "Student Test",
                PhoneNumber = "123456789",
                IdentityNumber = 123213
            };

            // Action
            var response = await _studentController.Add(studentAddParams) as OkObjectResult;

            //Assert
            Assert.Equal(200, response!.StatusCode);
        }
    }
}