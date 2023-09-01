using AutoMapper;
using ESM.Domain.Entities;
using ESM.Domain.Identity;

namespace ESM.Application.Auth.Queries.MySummaryInfo;

public class MySummaryInfoProfile : Profile
{
    public MySummaryInfoProfile()
    {
        CreateMap<Faculty, InternalFaculty>();
        CreateMap<Department, InternalDepartment>();
        CreateMap<Teacher, MySummaryInfoDto>();
        CreateMap<ApplicationUser, MySummaryInfoDto>();
    }
}