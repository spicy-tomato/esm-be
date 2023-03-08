using AutoMapper;
using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.Examination;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Dtos.Module;
using ESM.Data.Dtos.Room;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Request.Examination;
using ESM.Data.Request.Faculty;
using ESM.Data.Request.Module;
using ESM.Data.Request.Room;
using ESM.Data.Request.User;

namespace ESM.Domain.Mappings;

public class ModelMapping : Profile
{
    public ModelMapping()
    {
        #region Faculty

        CreateMap<CreateFacultyRequest, Faculty>();
        CreateMap<UpdateFacultyRequest, Faculty>();
        CreateMap<Faculty, FacultySummary>();
        CreateMap<Faculty, FacultyWithDepartments>();

        #endregion

        #region Department

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<UpdateDepartmentRequest, Department>();
        CreateMap<Department, DepartmentSummary>();
        CreateMap<Department, DepartmentSimple>();

        #endregion

        #region Examination

        CreateMap<CreateExaminationRequest, Examination>();
        CreateMap<Examination, ExaminationSummary>();
        CreateMap<Examination, ExaminationSimple>();

        CreateMap<ExaminationData, Shift>();

        CreateMap<Shift, ShiftSimple>();

        CreateMap<ShiftGroup, ShiftGroupSimple>();

        #endregion

        #region Module

        CreateMap<CreateModuleRequest, Module>();
        CreateMap<Module, ModuleSimple>();

        #endregion

        #region Room

        CreateMap<CreateRoomRequest, Room>();
        CreateMap<Room, RoomSummary>();

        #endregion

        #region User

        CreateMap<CreateUserRequest, User>()
           .ForMember(des => des.UserName,
                opt => opt.MapFrom(src => src.Email)
            );

        CreateMap<UpdateUserRequest, User>();
        CreateMap<User, UserSummary>()
           .ForMember(des => des.Role,
                opt => opt.MapFrom(src => src.Role.Name)
            );
        CreateMap<User, UserSimple>();

        #endregion
    }
}