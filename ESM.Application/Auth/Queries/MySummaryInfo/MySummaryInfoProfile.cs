using AutoMapper;
using ESM.Domain.Entities;

namespace ESM.Application.Auth.Queries.MySummaryInfo;

public class MySummaryInfoProfile : Profile
{
    public MySummaryInfoProfile()
    {
        CreateMap<Faculty, InternalFaculty>();
        CreateMap<Department, InternalDepartment>();
        CreateMap<Teacher, MySummaryInfoDto>();
    }
}