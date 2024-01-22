using AutoMapper;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Dtos.User;
using ESM.Domain.Entities;
using ESM.Domain.Identity;

namespace ESM.Application.Common.Mappings;

public class UserServiceProfile : Profile
{
    public UserServiceProfile()
    {
        CreateMap<ApplicationUser, UserSummary>();
        CreateMap<ApplicationUser, UserSimple>();          
        CreateMap<Examination, ExaminationSummary>();
    }
}