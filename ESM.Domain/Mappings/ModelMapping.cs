using AutoMapper;
using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Dtos.School;
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Request.Faculty;
using ESM.Data.Request.School;

namespace ESM.Domain.Mappings;

public class ModelMapping : Profile
{
    public ModelMapping()
    {
        #region School

        CreateMap<CreateSchoolRequest, School>();
        CreateMap<School, SimpleSchool>();

        #endregion
        
        #region Faculty

        CreateMap<CreateFacultyRequest, Faculty>();
        CreateMap<Faculty, SimpleFaculty>();

        #endregion

        #region Department

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<Department, SimpleDepartment>();

        #endregion
    }
}