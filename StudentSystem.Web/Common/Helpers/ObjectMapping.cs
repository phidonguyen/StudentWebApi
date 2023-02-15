using AutoMapper;
using StudentSystem.Web.Apis.Controllers.Params;
using StudentSystem.Web.Apis.Services.Auth;
using StudentSystem.Web.Apis.Services.Students;

namespace StudentSystem.Web.Common.Helpers
{
    public class ObjectMapping : Profile
    {
        public ObjectMapping()
        {
            AuthMapping();
            StudentMapping();
        }

        private void StudentMapping()
        {
            CreateMap<StudentsGetParams, StudentsGetServiceFields>();
            CreateMap<StudentAddParams, StudentAddServiceFields>();
            CreateMap<StudentUpdateParams, StudentUpdateServiceFields>();
        }
        
        private void AuthMapping()
        {
            CreateMap<AuthLoginParams, AuthLoginServiceFields>();
            CreateMap<AuthRefreshTokenParams, AuthRefreshTokenServiceFields>();
        }
    }
}
