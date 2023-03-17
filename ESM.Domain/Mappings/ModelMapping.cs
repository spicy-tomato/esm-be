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
using ESM.Data.Responses.Examination;
using ESM.Data.Responses.Faculty;

namespace ESM.Domain.Mappings;

public class ModelMapping : Profile
{
    public ModelMapping()
    {
        #region Faculty

        CreateMap<CreateFacultyRequest, Faculty>();
        CreateMap<UpdateFacultyRequest, Faculty>();

        CreateMap<Faculty, FacultySummary>();
        CreateMap<Faculty, GetAllResponseItem>();
        CreateMap<Faculty, GetAllGroupsResponseResponseItem.InternalFaculty>();
        CreateMap<Faculty, AssignInvigilatorNumerateOfShiftToFacultyResponse.InternalFaculty>();

        #endregion

        #region Department

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<UpdateDepartmentRequest, Department>();

        CreateMap<Department, DepartmentSummary>();
        CreateMap<Department, GetAllResponseItem.InternalDepartment>();

        #endregion

        #region Examination

        CreateMap<CreateExaminationRequest, Examination>();
        CreateMap<Examination, ExaminationSummary>();
        CreateMap<Examination, GetRelatedResponseItem>();
        CreateMap<Examination, GetAvailableInvigilatorsInShiftGroupResponse>();

        CreateMap<ExaminationData, Shift>();

        CreateMap<Shift, GetDataResponseItem>();
        CreateMap<Shift, GetShiftResponseItem>();

        CreateMap<ShiftGroup, GetDataResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetShiftResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetGroupByFacultyIdResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetAllGroupsResponseResponseItem>();
        CreateMap<ShiftGroup, AssignInvigilatorNumerateOfShiftToFacultyResponse>();
        CreateMap<ShiftGroup, GetAvailableInvigilatorsInShiftGroupResponse.InternalShiftGroup>();

        CreateMap<FacultyShiftGroup, GetShiftResponseItem.InternalFacultyShiftGroup>();
        CreateMap<FacultyShiftGroup, GetGroupByFacultyIdResponseItem.InternalFacultyShiftGroup>();
        CreateMap<FacultyShiftGroup, GetAvailableInvigilatorsInShiftGroupResponse.InternalFacultyShiftGroup>();

        CreateMap<DepartmentShiftGroup, GetShiftResponseItem.InternalDepartmentShiftGroup>();
        CreateMap<DepartmentShiftGroup, GetGroupByFacultyIdResponseItem>();
        CreateMap<DepartmentShiftGroup, GetAvailableInvigilatorsInShiftGroupResponse.InternalDepartmentShiftGroup>();

        CreateMap<InvigilatorShift, GetShiftResponseItem.InternalInvigilatorShift>();

        #endregion

        #region Module

        CreateMap<CreateModuleRequest, Module>();

        CreateMap<Module, ModuleSimple>();
        CreateMap<Module, GetDataResponseItem.InternalModule>();
        CreateMap<Module, GetGroupByFacultyIdResponseItem.InternalModule>();
        CreateMap<Module, GetAllGroupsResponseResponseItem.InternalModule>();
        CreateMap<Module, AssignInvigilatorNumerateOfShiftToFacultyResponse.InternalModule>();

        #endregion

        #region Room

        CreateMap<CreateRoomRequest, Room>();
        CreateMap<Room, RoomSummary>();
        CreateMap<Room, GetDataResponseItem.InternalRoom>();

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
        CreateMap<User, GetShiftResponseItem.InternalUser>();
        CreateMap<User, GetAvailableInvigilatorsInShiftGroupResponse.InternalUser>();

        #endregion
    }
}