using AutoMapper;
using TODOLISTTRY.DAL.Model;
using TODOLISTTRY.Services.DTO;

namespace TODOLISTTRY.Services.Profiles
{
    public class DoProfile : Profile
    {
        public DoProfile()
        {
            CreateMap<Do, DoDTO>().ReverseMap();
        }
    }
}
