using AutoMapper;
using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.Examination;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Dtos.School;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Request.Examination;
using ESM.Data.Request.Faculty;
using ESM.Data.Request.School;
using ESM.Data.Request.User;

namespace ESM.Domain.Mappings;

public class ModelMapping : Profile
{
    public ModelMapping()
    {
        #region User

        CreateMap<CreateUserRequest, User>();
        CreateMap<User, UserSummary>();

        #endregion

        #region School

        CreateMap<CreateSchoolRequest, School>();
        CreateMap<School, SchoolSummary>();

        #endregion

        #region Faculty

        CreateMap<CreateFacultyRequest, Faculty>();
        CreateMap<Faculty, FacultySummary>();
        CreateMap<Faculty, FacultyWithDepartments>();

        #endregion

        #region Department

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<Department, DepartmentSummary>();
        CreateMap<Department, DepartmentSimple>();

        #endregion

        #region Examination

        CreateMap<CreateExaminationRequest, Examination>();
        CreateMap<Examination, ExaminationSummary>();

        #endregion
    }
}