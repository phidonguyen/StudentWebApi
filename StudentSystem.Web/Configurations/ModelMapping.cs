using AutoMapper;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Apis.Models;

namespace StudentSystem.Web.Configurations
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            TokenMapping();
            StudentMapping();
            UserMapping();
        }

        private void UserMapping()
        {
            CreateMap<User, UserModel>()
                .ReverseMap();
        }

        private void TokenMapping()
        {
            CreateMap<Token, TokenModel>()
                .ReverseMap();
        }

        private void StudentMapping()
        {
            CreateMap<Student, StudentModel>()
                .ReverseMap();
        }
    }
}