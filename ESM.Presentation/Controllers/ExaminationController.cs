using System.Net;
using AutoMapper;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Helpers;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Examinations.Commands.AssignInvigilatorsToShifts;
using ESM.Application.Examinations.Commands.AutoAssignInvigilatorsToShifts;
using ESM.Application.Examinations.Commands.Create;
using ESM.Application.Examinations.Commands.Import;
using ESM.Application.Examinations.Commands.Update;
using ESM.Application.Examinations.Query.GetAllShifts;
using ESM.Application.Examinations.Query.GetAllShiftsDetails;
using ESM.Application.Examinations.Query.GetEvents;
using ESM.Application.Examinations.Query.GetHandoverData;
using ESM.Application.Examinations.Query.GetRelatedExaminations;
using ESM.Data.Dtos.Examination;
using ESM.Data.Enums;
using ESM.Data.Interfaces;
using ESM.Data.Request.Examination;
using ESM.Data.Responses.Examination;
using ESM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ExaminationController : ApiControllerBase
{
    #region Properties

    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;
    private const string NOT_FOUND_MESSAGE = "Examination ID does not exist!";

    #endregion

    #region Constructor

    public ExaminationController(IExaminationService examinationService, IApplicationDbContext context)
    {
        _examinationService = examinationService;
        _context = context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create new examination
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns> 
    [HttpPost]
    public async Task<Result<Guid>> Create(CreateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get related examinations of current user
    /// </summary>
    /// <returns></returns>
    [HttpGet("related")]
    public async Task<Result<List<RelatedExaminationDto>>> GetRelated(GetRelatedExaminationsQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get examination data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}")]
    public async Task<Result<List<ShiftInExaminationDto>>> GetAllShifts(string examinationId)
    {
        var query = new GetAllShiftsInExaminationQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("{examinationId}")]
    public async Task<Result<bool>> Import(ImportCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPatch("{examinationId}")]
    public async Task<Result<bool>> Update(UpdateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get events of examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/events")]
    public async Task<Result<List<ExaminationEvent>>> GetEvents(string examinationId)
    {
        var query = new GetEventsQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get handover data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/handover")]
    public async Task<Result<List<HandoverDataDto>>> GetHandoverData(string examinationId)
    {
        var query = new GetHandoverDataQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get all shifts in an examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/shift")]
    public async Task<Result<List<ShiftDetailsDto>>> GetShifts(string examinationId)
    {
        var query = new GetAllShiftsDetailsQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Update invigilators in shift
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift")]
    public async Task<Result<bool>> AssignInvigilatorsToShifts(string examinationId,
        [FromBody] AssignInvigilatorsToShiftsRequest request)
    {
        var command = new AssignInvigilatorsToShiftsCommand(examinationId, request);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update shift data (handover report, handover person)
    /// </summary>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift/{shiftId}")]
    public Result<bool> UpdateShift()
    {
        // Moved to /shift/{shiftId} 

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Auto assign teachers to shift group
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/shift/calculate")]
    public async Task<Result<bool>> AutoAssignTeachersToShift(string examinationId)
    {
        var command = new AutoAssignInvigilatorsToShiftsCommand(examinationId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Change examination status
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{examinationId}/status")]
    public async Task<Result<bool>> ChangeStatus(string examinationId,
        [FromBody] ChangeExaminationStatusRequest request)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId);
        var newStatus = request.Status;

        CheckNewStatusIsValid(entity.Status, newStatus);

        switch (entity.Status, newStatus)
        {
            case (ExaminationStatus.Setup, ExaminationStatus.AssignFaculty):
                FinishSetup(entity);
                break;
            case (ExaminationStatus.AssignFaculty, ExaminationStatus.AssignInvigilator):
                FinishAssignFaculty(entity);
                break;
            case (ExaminationStatus.AssignInvigilator, ExaminationStatus.Closed):
                FinishExamination(entity);
                break;
            default:
                throw new BadRequestException($"Unexpected condition (current: {entity.Status}, new: {newStatus})");
        }

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = newStatus
        });

        await _context.SaveChangesAsync();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Update exams number
    /// </summary>
    /// <param name="request"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/exams-number")]
    public Task<Result<bool>> UpdateExamsCount([FromBody] Dictionary<string, int> request, string examinationId)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.AssignFaculty);

        const string notFoundMessage = "Shift ID does not exists: ";

        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Shifts)
           .Load();

        foreach (var (shiftId, examsCount) in request)
        {
            if (!Guid.TryParse(shiftId, out var shiftGuid))
                throw new NotFoundException(notFoundMessage + shiftId);

            var found = false;
            foreach (var shiftGroup in entity.ShiftGroups)
            {
                if (shiftGroup.DepartmentAssign) continue;

                foreach (var shift in shiftGroup.Shifts)
                {
                    if (shift.Id != shiftGuid)
                        continue;

                    shift.ExamsCount = examsCount;
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new NotFoundException(notFoundMessage + shiftId);
        }

        _context.SaveChanges();

        return Task.FromResult(Result<bool>.Get(true));
    }

    /// <summary>
    /// Get shift by faculty ID
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/faculty/{facultyId}/group")]
    public Result<IQueryable<GetGroupByFacultyIdResponseItem>> GetGroupsByFacultyId(string examinationId,
        string facultyId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId,
            ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);
        var facultyGuid = ParseGuid(facultyId);
        var data = Mapper.ProjectTo<GetGroupByFacultyIdResponseItem>(
            _context.DepartmentShiftGroups
               .Where(fg =>
                    fg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid &&
                    fg.FacultyShiftGroup.FacultyId == facultyGuid
                )
               .Include(dg => dg.FacultyShiftGroup)
               .ThenInclude(fg => fg.ShiftGroup)
               .OrderBy(eg => eg.FacultyShiftGroup.ShiftGroup.StartAt)
        );

        return Result<IQueryable<GetGroupByFacultyIdResponseItem>>.Get(data);
    }

    /// <summary>
    /// Update teacher assignment
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/faculty/{facultyId}/group")]
    public Result<bool> UpdateTeacherAssignment(string examinationId,
        string facultyId,
        [FromBody] Dictionary<string, UpdateTeacherAssignmentRequestElement> data)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.AssignInvigilator);
        var facultyGuid = ParseGuid(facultyId);
        var facultyShiftGroups = _context.DepartmentShiftGroups
           .Where(fg =>
                fg.FacultyShiftGroup.FacultyId == facultyGuid &&
                fg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid)
           .Include(dg => dg.FacultyShiftGroup)
           .ThenInclude(fg => fg.ShiftGroup)
           .AsEnumerable()
           .ToDictionary(fg => fg.Id.ToString(), fg => fg);

        foreach (var (departmentShiftGroupId, rowData) in data)
        {
            if (!facultyShiftGroups.TryGetValue(departmentShiftGroupId, out var departmentShiftGroup))
                throw new BadRequestException(
                    $"Department group ID {departmentShiftGroupId} is not exist in faculty group ID {facultyId}");

            if (rowData.DepartmentId != null)
                departmentShiftGroup.DepartmentId = new Guid(rowData.DepartmentId);
            if (rowData.UserId != null)
            {
                departmentShiftGroup.UserId = new Guid(rowData.UserId);
                departmentShiftGroup.TemporaryInvigilatorName = null;
            }
            else if (rowData.TemporaryInvigilatorName != null)
            {
                departmentShiftGroup.TemporaryInvigilatorName = rowData.TemporaryInvigilatorName;
                departmentShiftGroup.UserId = null;
            }
        }

        foreach (var facultyShiftGroup in facultyShiftGroups.Select(fg => fg.Value.FacultyShiftGroup))
        {
            var selectedTeachers = new Dictionary<Guid, bool>();
            foreach (var userId in facultyShiftGroup.DepartmentShiftGroups.Select(dg => dg.UserId))
            {
                if (userId == null)
                    continue;

                if (selectedTeachers.ContainsKey(userId.Value))
                    throw new BadRequestException(
                        $"User ID {userId} is selected more than one time in faculty group ID {facultyShiftGroup.Id}");

                selectedTeachers.Add(userId.Value, true);
            }
        }

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Auto assign teachers in faculty to shift groups
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpPost("{examinationId}/faculty/{facultyId}/group/calculate")]
    public Result<bool> AutoAssignTeachersToShiftGroups(string examinationId, string facultyId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.AssignInvigilator);
        var facultyGuid = ParseGuid(facultyId);
        if (_context.Faculties.FirstOrDefault(f => f.Id == facultyGuid) == null)
            throw new NotFoundException("Faculty ID does not exist!");

        var allTeachersInFaculty = _context.Teachers
           .Where(u => u.Department != null && u.Department.FacultyId == facultyGuid)
           .Include(u => u.Department)
           .ToList();

        var shiftGroups = _context.DepartmentShiftGroups
           .Where(dg =>
                dg.FacultyShiftGroup.FacultyId == facultyGuid &&
                dg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid)
           .Include(dg => dg.FacultyShiftGroup.ShiftGroup)
           .ThenInclude(g => g.Module)
           .OrderBy(dg => dg.FacultyShiftGroup.ShiftGroup.StartAt)
           .ThenBy(dg => dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId)
           .ToList();

        var minimumAppearance = shiftGroups.Count / allTeachersInFaculty.Count;
        var minIndexToRandom = minimumAppearance * allTeachersInFaculty.Count;

        for (var i = 0; i < minIndexToRandom; i++)
        {
            var departmentShiftGroup = shiftGroups[i];
            var invigilatorIndex = i % allTeachersInFaculty.Count;
            var invigilator = allTeachersInFaculty[invigilatorIndex];

            departmentShiftGroup.UserId = invigilator.User.Id;
            departmentShiftGroup.DepartmentId = invigilator.DepartmentId;
        }

        for (var i = minIndexToRandom; i < shiftGroups.Count; i++)
        {
            var departmentShiftGroup = shiftGroups[i];
            var invigilatorIndex = RandomHelper.Next(allTeachersInFaculty.Count);
            var invigilator = allTeachersInFaculty[invigilatorIndex];

            departmentShiftGroup.UserId = invigilator.User.Id;
            departmentShiftGroup.DepartmentId = invigilator.DepartmentId;
            allTeachersInFaculty.RemoveAt(invigilatorIndex);
        }

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Get all shift groups in examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/group")]
    public Result<List<GetAllGroupsResponseResponseItem>> GetAllGroups(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);
        var data = Mapper.ProjectTo<GetAllGroupsResponseResponseItem>(
            _context.ShiftGroups
               .Include(eg => eg.FacultyShiftGroups)
               .Where(e => e.ExaminationId == guid && !e.DepartmentAssign)
               .OrderBy(eg => eg.StartAt)
        ).ToList();
        var facultyShiftGroup = _context.FacultyShiftGroups
           .Where(fg => fg.ShiftGroup.ExaminationId == guid && !fg.ShiftGroup.DepartmentAssign);
        var invigilatorsNumberInFaculties = _userRepository.CountByFaculties();

        foreach (var group in data)
        {
            CalculateShiftInvigilatorsNumber(group,
                facultyShiftGroup.Where(fg => fg.ShiftGroupId == group.Id).ToList(),
                invigilatorsNumberInFaculties);
        }

        return Result<List<GetAllGroupsResponseResponseItem>>.Get(data);
    }

    /// <summary>
    /// Calculate invigilators number of shift for each faculty
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/group/calculate")]
    public Result<bool> CalculateInvigilatorNumerateOfShiftForEachFaculty(string examinationId)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.AssignFaculty);

        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Module)
           .Include(eg => eg.FacultyShiftGroups)
           .Load();

        var faculties = _facultyRepository.GetAll().ToList();
        var teachersNumberInFaculties = GetTeachersNumberInFaculties();
        var teachersTotal = teachersNumberInFaculties.Sum(t => t.Value);

        foreach (var group in entity.ShiftGroups)
        {
            var mainFacultyId = group.Module.FacultyId;
            var teachersNumberInRestFaculties =
                teachersTotal - teachersNumberInFaculties.GetValueOrDefault(mainFacultyId, 0);

            foreach (var facultyId in faculties.Select(f => f.Id))
            {
                var calculatedInvigilatorsCount = facultyId == mainFacultyId
                    ? group.RoomsCount
                    : Convert.ToInt32((group.InvigilatorsCount - group.RoomsCount) *
                                      (teachersNumberInFaculties.GetValueOrDefault(facultyId, 0) * 1.0 /
                                       teachersNumberInRestFaculties));
                var savedRecord = group.FacultyShiftGroups
                   .FirstOrDefault(feg => feg.FacultyId == facultyId);
                if (savedRecord == null)
                    group.FacultyShiftGroups.Add(new FacultyShiftGroup
                    {
                        FacultyId = facultyId,
                        ShiftGroup = group,
                        InvigilatorsCount = calculatedInvigilatorsCount,
                        CalculatedInvigilatorsCount = calculatedInvigilatorsCount
                    });
                else
                {
                    savedRecord.InvigilatorsCount = calculatedInvigilatorsCount;
                    savedRecord.CalculatedInvigilatorsCount = calculatedInvigilatorsCount;
                }
            }
        }

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    [HttpPost("{examinationId}/group/{groupId}/department/{departmentId}")]
    public Result<bool> UpdateTemporaryTeacherToUserIdInDepartmentShiftGroup([FromBody] dynamic request,
        string examinationId,
        string groupId,
        string departmentId)
    {
        const string notFoundUserMessage = "User ID does not exist!";
        const string notFoundGroupMessage = "Group ID does not exist!";
        const string notFoundFacultyMessage = "Department ID does not exist!";

        if (!Guid.TryParse((string)request.userId, out var userGuid))
            throw new NotFoundException(notFoundUserMessage);
        if (!Guid.TryParse(groupId, out var groupGuid))
            throw new NotFoundException(notFoundGroupMessage);
        if (!Guid.TryParse(departmentId, out var departmentGuid))
            throw new NotFoundException(notFoundFacultyMessage);

        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.AssignInvigilator);

        var departmentShiftGroup =
            _context.DepartmentShiftGroups.FirstOrDefault(dsg =>
                dsg.DepartmentId == departmentGuid &&
                dsg.FacultyShiftGroup.ShiftGroupId == groupGuid &&
                dsg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid
            );

        if (departmentShiftGroup == null)
            throw new BadRequestException("Data is invalid");

        departmentShiftGroup.UserId = userGuid;
        departmentShiftGroup.TemporaryInvigilatorName = null;

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Assign invigilator number of a shift for a faculty
    /// </summary>
    /// <param name="numberOfInvigilator"></param>
    /// <param name="examinationId"></param>
    /// <param name="groupId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/group/{groupId}/{facultyId}")]
    public Result<AssignInvigilatorNumerateOfShiftToFacultyResponse?> AssignInvigilatorNumerateOfShiftToFaculty(
        [FromBody] int numberOfInvigilator,
        string examinationId,
        string groupId,
        string facultyId)
    {
        if (numberOfInvigilator < 0)
            throw new BadRequestException("Number cannot be negative!");

        const string notFoundGroupMessage = "Group ID does not exist!";
        const string notFoundFacultyMessage = "Faculty ID does not exist!";

        if (!Guid.TryParse(groupId, out var groupGuid))
            throw new NotFoundException(notFoundGroupMessage);
        if (!Guid.TryParse(facultyId, out var facultyGuid))
            throw new NotFoundException(notFoundFacultyMessage);

        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.AssignFaculty);

        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.FacultyShiftGroups)
           .Load();

        var group = entity.ShiftGroups.FirstOrDefault(eg => eg.Id == groupGuid);
        if (group == null)
            throw new NotFoundException(notFoundGroupMessage);

        var facultyGroup = group.FacultyShiftGroups.FirstOrDefault(eg => eg.FacultyId == facultyGuid);
        if (facultyGroup == null)
        {
            facultyGroup = new FacultyShiftGroup
            {
                FacultyId = facultyGuid,
                ShiftGroup = group
            };
            group.FacultyShiftGroups.Add(facultyGroup);
        }

        facultyGroup.InvigilatorsCount = numberOfInvigilator;

        _context.SaveChanges();

        var data = Mapper.ProjectTo<AssignInvigilatorNumerateOfShiftToFacultyResponse>(
            _context.ShiftGroups
               .Include(eg => eg.FacultyShiftGroups)
               .Where(e => e.ExaminationId == entity.Id && !e.DepartmentAssign && e.Id == groupGuid)
        ).FirstOrDefault();

        var invigilatorsNumberInFaculties = _userRepository.CountByFaculties();

        var facultyShiftGroup = _context.FacultyShiftGroups
           .Where(fg => fg.ShiftGroupId == groupGuid && !fg.ShiftGroup.DepartmentAssign)
           .ToList();

        if (data != null)
            CalculateShiftInvigilatorsNumber(data, facultyShiftGroup, invigilatorsNumberInFaculties);

        return Result<AssignInvigilatorNumerateOfShiftToFacultyResponse?>.Get(data);
    }

    /// <summary>
    /// Get invigilators for each shift group
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpGet("{examinationId}/invigilator")]
    public Result<Dictionary<string, List<GetAvailableInvigilatorsInShiftGroup.ResponseItem>>>
        GetAvailableInvigilatorsInShiftGroup(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId);

        var shiftGroups =
            Mapper.ProjectTo<GetAvailableInvigilatorsInShiftGroup>(
                _context.ShiftGroups
                   .Where(s => s.ExaminationId == guid)
                   .Include(g => g.FacultyShiftGroups)
                   .ThenInclude(fg => fg.DepartmentShiftGroups)
                   .ThenInclude(dg => dg.User)
            );

        var priorityFacultyOfShiftGroupsQuery = _context.ShiftGroups
           .Where(g => g.ExaminationId == guid)
           .Include(g => g.Module)
           .Select(g => new { g.Id, g.Module.FacultyId })
           .AsSplitQuery()
           .ToDictionary(g => g.Id, g => g.FacultyId);

        var result =
            new Dictionary<string, List<GetAvailableInvigilatorsInShiftGroup.ResponseItem>>();

        foreach (var group in shiftGroups)
        {
            var list = new List<GetAvailableInvigilatorsInShiftGroup.ResponseItem>();
            var groupId = group.Id.ToString();

            foreach (var facultyShiftGroup in group.FacultyShiftGroups)
            {
                var priorityFacultyId = priorityFacultyOfShiftGroupsQuery[group.Id];
                foreach (var departmentShiftGroup in facultyShiftGroup.DepartmentShiftGroups)
                {
                    if (departmentShiftGroup.User == null &&
                        departmentShiftGroup.TemporaryInvigilatorName.IsNullOrEmpty())
                        continue;

                    var isPriority = facultyShiftGroup.FacultyId == priorityFacultyId;
                    GetAvailableInvigilatorsInShiftGroup.ResponseItem item = departmentShiftGroup.User == null
                        ? new GetAvailableInvigilatorsInShiftGroup.TemporaryInvigilator
                        {
                            TemporaryName = departmentShiftGroup.TemporaryInvigilatorName!,
                            DepartmentId = departmentShiftGroup.DepartmentId,
                            IsPriority = isPriority
                        }
                        : new GetAvailableInvigilatorsInShiftGroup.VerifiedInvigilator
                        {
                            Id = departmentShiftGroup.User.Id,
                            FullName = departmentShiftGroup.User.FullName,
                            InvigilatorId = departmentShiftGroup.User.InvigilatorId,
                            IsPriority = isPriority,
                            PhoneNumber = departmentShiftGroup.User.PhoneNumber,
                            FacultyName = departmentShiftGroup.User.Department?.Faculty?.Name,
                            DepartmentName = departmentShiftGroup.User.Department?.Name
                        };

                    list.Add(item);
                }
            }

            result.Add(groupId, list);
        }

        return Result<bool>.Get(result);
    }

    /// <summary>
    /// Get summary
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns> 
    [HttpGet("{examinationId}/summary")]
    public Result<ExaminationSummary> GetSummary(string examinationId)
    {
        var guid = ParseGuid(examinationId);
        var createdExamination = _examinationRepository.GetById(guid);
        if (createdExamination == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        return Result<ExaminationSummary>.Get(createdExamination);
    }

    /// <summary>
    /// Get temporary data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/temporary")]
    public Result<IQueryable<ExaminationData>> GetTemporaryData(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Setup);

        var data = GetTemporaryData(guid);
        return Result<IQueryable<ExaminationData>>.Get(data);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Get temporary data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="skipValidate"></param>
    /// <returns></returns>
    private IQueryable<ExaminationData> GetTemporaryData(Guid examinationId, bool skipValidate = false)
    {
        var data = _context.ExaminationData.Where(e => e.ExaminationId == examinationId);
        if (!skipValidate && data.Any())
            data = _examinationService.ValidateTemporaryData(data);

        return data;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private static Guid ParseGuid(string id, string? message = null)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new NotFoundException(message ?? NOT_FOUND_MESSAGE);
        return guid;
    }

    private Guid CheckIfExaminationExistAndReturnGuid(string examinationId, ExaminationStatus? acceptStatus = null)
    {
        var guid = ParseGuid(examinationId);
        var status = _examinationRepository.GetStatus(guid);

        if (status == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (acceptStatus != null && (acceptStatus.Value & status) == 0)
            throw new BadRequestException($"Examination status should be {acceptStatus.ToString()}");
        return guid;
    }

    private Examination CheckIfExaminationExistAndReturnEntity(string examinationId,
        ExaminationStatus? acceptStatus = null)
    {
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations.FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (acceptStatus != null && (acceptStatus.Value & entity.Status) == 0)
            throw new BadRequestException($"Examination status should be {acceptStatus.ToString()}");
        return entity;
    }

    private Dictionary<Guid, int> GetTeachersNumberInFaculties()
    {
        var teachers = _userRepository.Find(u => u.DepartmentId != null).ToList();
        var facultyTeachersCount = new Dictionary<Guid, int>();

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var teacher in teachers)
        {
            var faculty = teacher.Department!.Faculty;
            if (faculty == null)
                continue;

            var facultyId = faculty.Id;
            if (facultyTeachersCount.ContainsKey(facultyId))
                facultyTeachersCount[facultyId]++;
            else
                facultyTeachersCount.Add(facultyId, 1);
        }

        return facultyTeachersCount;
    }

    private static void CalculateShiftInvigilatorsNumber<T>(T group,
        ICollection<FacultyShiftGroup> facultyShiftGroup,
        IReadOnlyDictionary<Guid, int> invigilatorsNumberInFaculties) where T : IShiftGroup
    {
        group.AssignNumerate = facultyShiftGroup
           .ToDictionary(
                fg => fg.FacultyId.ToString(),
                fg => new ShiftGroupDataCell
                {
                    Actual = fg.InvigilatorsCount,
                    Calculated = fg.CalculatedInvigilatorsCount,
                    Maximum = invigilatorsNumberInFaculties.GetValueOrDefault(fg.FacultyId, 0)
                }
            );

        var total = facultyShiftGroup.Sum(feg => feg.InvigilatorsCount);
        group.AssignNumerate.Add("total",
            new ShiftGroupDataCell
            {
                // Actual calculation result
                Actual = total,
                // Difference
                Calculated = total - group.InvigilatorsCount
            });
    }

    private static void CheckNewStatusIsValid(ExaminationStatus currentStatus, ExaminationStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(ExaminationStatus), newStatus))
            throw new BadRequestException("New status is invalid");

        // Key  : Valid current status
        // Value: Valid new status
        var statusMap = new Dictionary<ExaminationStatus, ExaminationStatus[]>
        {
            { ExaminationStatus.Setup, new[] { ExaminationStatus.AssignFaculty } },
            { ExaminationStatus.AssignFaculty, new[] { ExaminationStatus.AssignInvigilator } },
            { ExaminationStatus.AssignInvigilator, new[] { ExaminationStatus.AssignFaculty, ExaminationStatus.Closed } }
        };

        foreach (var (validCurrentStatus, validNewStatuses) in statusMap)
        {
            if (currentStatus == validCurrentStatus && validNewStatuses.Contains(newStatus))
                return;
        }

        var expectedCurrentStatusSatisfiesNewStatus = statusMap.Where(item => item.Value.Contains(newStatus)).ToList();
        if (expectedCurrentStatusSatisfiesNewStatus.IsNullOrEmpty())
            throw new BadRequestException($"Cannot change status to {newStatus} (current: {currentStatus})");

        var expectedCurrentStatusSatisfiesNewStatusStr =
            string.Join(", ", expectedCurrentStatusSatisfiesNewStatus.Select(s => s.Key));
        throw new BadRequestException($"Examination status should be {expectedCurrentStatusSatisfiesNewStatusStr}");
    }

    private void FinishSetup(Examination entity)
    {
        var temporaryData = GetTemporaryData(entity.Id, true);
        var shifts = _examinationService.RetrieveShiftsFromTemporaryData(entity.Id, temporaryData);

        _shiftRepository.CreateRangeAsync(shifts);
        entity.Status = ExaminationStatus.AssignFaculty;
    }

    private void FinishAssignFaculty(Examination entity)
    {
        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.FacultyShiftGroups)
           .Include(eg => eg.Module)
           .Where(eg => !eg.DepartmentAssign)
           .Load();

        foreach (var group in entity.ShiftGroups)
        {
            var expected = group.InvigilatorsCount;
            var actual = group.FacultyShiftGroups.Sum(feg => feg.InvigilatorsCount);
            if (actual != expected)
                throw new BadRequestException(
                    $"Actual assigned invigilators number is not match (met {actual}, need {expected}) in group {group.Id} ({group.Module.Name})");
        }

        var oldDepartmentShiftGroups =
            _context.DepartmentShiftGroups.Where(dg => dg.FacultyShiftGroup.ShiftGroup.ExaminationId == entity.Id);
        _context.DepartmentShiftGroups.RemoveRange(oldDepartmentShiftGroups);

        foreach (var group in entity.ShiftGroups)
        {
            foreach (var facultyGroup in group.FacultyShiftGroups)
            {
                facultyGroup.DepartmentShiftGroups =
                    new List<DepartmentShiftGroup>(new DepartmentShiftGroup[facultyGroup.InvigilatorsCount])
                       .Select(_ => new DepartmentShiftGroup { FacultyShiftGroup = facultyGroup })
                       .ToList();
            }
        }

        entity.Status = ExaminationStatus.AssignInvigilator;
    }

    private void FinishExamination(Examination entity)
    {
        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Shifts)
           .ThenInclude(s => s.Room)
           .Where(eg => !eg.DepartmentAssign)
           .Load();

        var shifts = entity.ShiftGroups.SelectMany(sg => sg.Shifts);
        var notHandedOverShifts = shifts.Where(s => s.HandedOverUserId == null).ToList();

        if (notHandedOverShifts.Any())
        {
            var errorList = notHandedOverShifts.Select(s =>
                new Error(
                    HttpStatusCode.BadRequest,
                    $"Shift have not been handed over yet: Date {s.ShiftGroup.StartAt}, module {s.ShiftGroup.ModuleId}, room {s.Room.DisplayId}")
            );
            throw new HttpException(HttpStatusCode.Conflict, errorList);
        }


        entity.Status = ExaminationStatus.Closed;
    }

    #endregion
}