using AutoMapper;
using ESM.Domain.Dtos.Department;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Dtos.Faculty;
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
        CreateMap<Teacher, UserSummary>();
        CreateMap<Examination, ExaminationSummary>();
        CreateMap<Department, DepartmentSummary>();
        CreateMap<Faculty, FacultySummary>();
    }
}