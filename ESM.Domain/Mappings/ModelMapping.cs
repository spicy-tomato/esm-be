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
        CreateMap<Faculty, GetShiftResponseItem.InternalFaculty>();
        CreateMap<Faculty, GetHandoverDataResponseItem.InternalFaculty>();
        CreateMap<Faculty, GetAllGroupsResponseResponseItem.InternalFaculty>();
        CreateMap<Faculty, AssignInvigilatorNumerateOfShiftToFacultyResponse.InternalFaculty>();
        CreateMap<Faculty, GetAvailableInvigilatorsInShiftGroup.InternalFaculty>();

        #endregion

        #region Department

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<UpdateDepartmentRequest, Department>();

        CreateMap<Department, DepartmentSummary>();
        CreateMap<Department, GetAllResponseItem.InternalDepartment>();
        CreateMap<Department, GetShiftResponseItem.InternalDepartment>();
        CreateMap<Department, GetHandoverDataResponseItem.InternalDepartment>();
        CreateMap<Department, GetAvailableInvigilatorsInShiftGroup.InternalDepartment>();

        #endregion

        #region Examination

        CreateMap<CreateExaminationRequest, Examination>();
        CreateMap<Examination, ExaminationSummary>();
        CreateMap<Examination, GetRelatedResponseItem>();

        CreateMap<ExaminationData, Shift>();

        CreateMap<Shift, GetDataResponseItem>();
        CreateMap<Shift, GetShiftResponseItem>();
        CreateMap<Shift, GetHandoverDataResponseItem>();

        CreateMap<ShiftGroup, GetDataResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetShiftResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetHandoverDataResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetGroupByFacultyIdResponseItem.InternalShiftGroup>();
        CreateMap<ShiftGroup, GetAllGroupsResponseResponseItem>();
        CreateMap<ShiftGroup, GetAvailableInvigilatorsInShiftGroup>();
        CreateMap<ShiftGroup, AssignInvigilatorNumerateOfShiftToFacultyResponse>();

        CreateMap<FacultyShiftGroup, GetGroupByFacultyIdResponseItem.InternalFacultyShiftGroup>();
        CreateMap<FacultyShiftGroup, GetAvailableInvigilatorsInShiftGroup.InternalFacultyShiftGroup>();

        CreateMap<DepartmentShiftGroup, GetGroupByFacultyIdResponseItem>();
        CreateMap<DepartmentShiftGroup, GetAvailableInvigilatorsInShiftGroup.InternalDepartmentShiftGroup>();

        CreateMap<InvigilatorShift, GetShiftResponseItem.InternalInvigilatorShift>();
        CreateMap<InvigilatorShift, GetHandoverDataResponseItem.InternalInvigilatorShift>();

        #endregion

        #region Module

        CreateMap<CreateModuleRequest, Module>();

        CreateMap<Module, ModuleSimple>();
        CreateMap<Module, GetDataResponseItem.InternalModule>();
        CreateMap<Module, GetHandoverDataResponseItem.InternalModule>();
        CreateMap<Module, GetGroupByFacultyIdResponseItem.InternalModule>();
        CreateMap<Module, GetAllGroupsResponseResponseItem.InternalModule>();
        CreateMap<Module, AssignInvigilatorNumerateOfShiftToFacultyResponse.InternalModule>();

        #endregion

        #region Room

        CreateMap<CreateRoomRequest, Room>();
        CreateMap<Room, RoomSummary>();
        CreateMap<Room, GetDataResponseItem.InternalRoom>();
        CreateMap<Room, GetHandoverDataResponseItem.InternalRoom>();

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
        CreateMap<User, GetHandoverDataResponseItem.InternalUser>();
        CreateMap<User, GetAvailableInvigilatorsInShiftGroup.InternalUser>();

        #endregion
    }
}