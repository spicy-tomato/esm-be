using System.Net;
using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core;
using ESM.Common.Core.Exceptions;
using ESM.Common.Core.Helpers;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Examination;
using ESM.Data.Enums;
using ESM.Data.Interfaces;
using ESM.Data.Models;
using ESM.Data.Request.Examination;
using ESM.Data.Responses.Examination;
using ESM.Data.Validations.Examination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ExaminationController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly ExaminationDataRepository _examinationDataRepository;
    private readonly ExaminationRepository _examinationRepository;
    private readonly ShiftRepository _shiftRepository;
    private readonly FacultyRepository _facultyRepository;
    private readonly UserRepository _userRepository;
    private readonly ExaminationService _examinationService;
    private const string NOT_FOUND_MESSAGE = "Examination ID does not exist!";

    #endregion

    #region Constructor

    public ExaminationController(IMapper mapper,
        ExaminationRepository examinationRepository,
        ExaminationDataRepository examinationDataRepository,
        ExaminationService examinationService,
        ApplicationContext context,
        ShiftRepository shiftRepository,
        FacultyRepository facultyRepository,
        UserRepository userRepository) : base(mapper)
    {
        _examinationRepository = examinationRepository;
        _examinationDataRepository = examinationDataRepository;
        _examinationService = examinationService;
        _context = context;
        _shiftRepository = shiftRepository;
        _facultyRepository = facultyRepository;
        _userRepository = userRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create new examination
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns> 
    [HttpPost]
    public async Task<Result<ExaminationSummary>> Create(CreateExaminationRequest request)
    {
        await new CreateExaminationRequestValidator().ValidateAndThrowAsync(request);
        var examination = Mapper.Map<Examination>(request);
        examination.Status = ExaminationStatus.Idle;
        examination.CreatedById = GetUserId();

        var createdExamination = _examinationRepository.Create(examination);
        var response = Mapper.Map<ExaminationSummary>(createdExamination);

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = createdExamination.Id,
            Status = ExaminationStatus.Idle
        });

        await _context.SaveChangesAsync();

        return Result<ExaminationSummary>.Get(response);
    }

    /// <summary>
    /// Get related examinations of current user
    /// </summary>
    /// <returns></returns>
    [HttpGet("related")]
    public Result<List<GetRelatedResponseItem>> GetRelated([FromQuery] bool? isActive)
    {
        var query = isActive switch
        {
            null => _context.Examinations,
            true => _context.Examinations.Where(e => 0 < e.Status && e.Status < ExaminationStatus.Closed),
            _ => _context.Examinations.Where(e => e.Status == ExaminationStatus.Closed)
        };

        var createdExamination = Mapper.ProjectTo<GetRelatedResponseItem>(query.OrderBy(e => e.CreatedAt))
           .ToList();

        return Result<List<GetRelatedResponseItem>>.Get(createdExamination);
    }

    /// <summary>
    /// Get examination data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}")]
    public Result<IQueryable<GetDataResponseItem>> GetData(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator | ExaminationStatus.Closed);

        var data = Mapper.ProjectTo<GetDataResponseItem>(
            _context.Shifts
               .Where(e =>
                    e.ShiftGroup.ExaminationId == guid &&
                    !e.ShiftGroup.DepartmentAssign)
               .OrderBy(s => s.ShiftGroup.StartAt)
               .ThenBy(s => s.ShiftGroupId)
               .ThenBy(s => s.ShiftGroup.Module.Name)
               .ThenBy(s => s.Room.DisplayId)
        );

        return Result<IQueryable<GetDataResponseItem>>.Get(data);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("{examinationId}")]
    public async Task<Result<bool>> Import(string examinationId, [FromForm] ImportExaminationRequest request)
    {
        if (request.File == null)
            throw new BadRequestException("File should not be empty");

        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.Idle);
        var file = request.File;

        List<ExaminationData> readDataResult;
        try
        {
            readDataResult = ExaminationService.Import(file, examinationId);
        }
        catch (OpenXmlPackageException)
        {
            throw new UnsupportedMediaTypeException();
        }

        _examinationDataRepository.CreateRange(readDataResult);
        entity.Status = ExaminationStatus.Setup;

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = ExaminationStatus.Setup
        });

        await _context.SaveChangesAsync();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPatch("{examinationId}")]
    public Result<bool> Update(string examinationId, [FromBody] UpdateExaminationRequest request)
    {
        new UpdateExaminationRequestValidator().ValidateAndThrow(request);
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId);

        entity.DisplayId = request.DisplayId ?? entity.DisplayId;
        entity.Name = request.Name ?? entity.Name;
        entity.Description = request.Description ?? entity.Description;
        entity.ExpectStartAt = request.ExpectStartAt ?? entity.ExpectStartAt;
        entity.ExpectEndAt = request.ExpectEndAt ?? entity.ExpectEndAt;
        entity.UpdatedAt = request.UpdatedAt;

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Get events of examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/events")]
    public Result<IQueryable<ExaminationEvent>> GetEvents(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId);
        var data = _context.ExaminationEvents.Where(e => e.ExaminationId == guid);
        return Result<IQueryable<ExaminationEvent>>.Get(data);
    }

    /// <summary>
    /// Get handover data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/handover")]
    public Result<IQueryable<GetHandoverDataResponseItem>> GetHandoverData(string examinationId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId);

        var data =
            Mapper.ProjectTo<GetHandoverDataResponseItem>(
                _context.Shifts
                   .Include(s => s.InvigilatorShift)
                   .ThenInclude(i => i.Invigilator)
                   .Include(s => s.Room)
                   .Include(s => s.ShiftGroup)
                   .ThenInclude(g => g.Module)
                   .Include(s => s.ShiftGroup)
                   .Where(g => g.ShiftGroup.ExaminationId == examinationGuid && !g.ShiftGroup.DepartmentAssign)
                   .OrderBy(g => g.ShiftGroup.StartAt)
                   .ThenBy(g => g.ShiftGroup.ModuleId)
                   .ThenBy(g => g.RoomId)
            );

        return Result<IQueryable<GetHandoverDataResponseItem>>.Get(data);
    }

    /// <summary>
    /// Get all shifts in an examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/shift")]
    public Result<List<GetShiftResponseItem>> GetShifts(string examinationId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId);
        var data =
            Mapper.ProjectTo<GetShiftResponseItem>(
                _context.Shifts
                   .Include(s => s.InvigilatorShift)
                   .ThenInclude(i => i.Invigilator)
                   .ThenInclude(u => u!.Department)
                   .ThenInclude(d => d!.Faculty)
                   .Include(s => s.Room)
                   .Include(s => s.ShiftGroup)
                   .ThenInclude(g => g.Module)
                   .Where(g => g.ShiftGroup.ExaminationId == examinationGuid && !g.ShiftGroup.DepartmentAssign)
                   .OrderBy(g => g.ShiftGroup.StartAt)
                   .ThenBy(g => g.ShiftGroup.ModuleId)
                   .ThenBy(g => g.RoomId)
            ).AsEnumerable().ToList();

        var duplicatedShift = _context.Shifts
           .Select(s => new
            {
                s.RoomId,
                ShiftGroup = new
                {
                    s.ShiftGroup.ExaminationId,
                    s.ShiftGroup.StartAt,
                    s.ShiftGroup.DepartmentAssign,
                    s.ShiftGroup.ModuleId
                }
            })
           .Where(s => s.ShiftGroup.ExaminationId == examinationGuid && !s.ShiftGroup.DepartmentAssign)
           .OrderBy(s => s.ShiftGroup.StartAt)
           .ThenBy(s => s.ShiftGroup.ModuleId)
           .ThenBy(s => s.RoomId)
           .GroupBy(s => new { s.ShiftGroup.StartAt, s.ShiftGroup.ModuleId, s.RoomId })
           .Select(r => new
            {
                r.Key.StartAt,
                r.Key.ModuleId,
                r.Key.RoomId,
                Count = r.Count()
            })
           .Where(r => r.Count > 1)
           .ToList();

        foreach (var shift in data)
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var ds in duplicatedShift)
            {
                if (ds.StartAt < shift.ShiftGroup.StartAt)
                    continue;
                if (ds.StartAt > shift.ShiftGroup.StartAt)
                    break;
                if (shift.Room.Id != ds.RoomId || shift.ShiftGroup.Module.Id != ds.ModuleId)
                    continue;

                shift.IsDuplicated = true;
                break;
            }
        }

        return Result<List<GetShiftResponseItem>>.Get(data);
    }

    /// <summary>
    /// Update invigilators in shift
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift")]
    public Result<bool> AssignInvigilatorsToShifts(string examinationId,
        [FromBody] AssignInvigilatorsToShiftsRequest request)
    {
        if (request.IsNullOrEmpty())
            return Result<bool>.Get(true);

        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.AssignInvigilator);

        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Shifts)
           .ThenInclude(fg => fg.InvigilatorShift)
           .AsSplitQuery()
           .Load();

        foreach (var shiftGroup in entity.ShiftGroups)
            foreach (var shift in shiftGroup.Shifts)
                foreach (var invigilatorShift in shift.InvigilatorShift)
                {
                    if (!request.TryGetValue(invigilatorShift.Id.ToString(), out var invigilatorId))
                        continue;

                    if (invigilatorId == null)
                    {
                        invigilatorShift.InvigilatorId = null;
                        request.Remove(invigilatorShift.Id.ToString());
                        continue;
                    }

                    if (!Guid.TryParse(invigilatorId, out var invigilatorGuid))
                        throw new BadRequestException($"Cannot parse invigilator ID to Guid: {invigilatorId}");

                    invigilatorShift.InvigilatorId = invigilatorGuid;
                    request.Remove(invigilatorShift.Id.ToString());
                }

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Update shift data (handover report, handover person)
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="shiftId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift/{shiftId}")]
    public Result<bool> UpdateShift(string examinationId,
        string shiftId,
        [FromBody] UpdateShiftRequest request)
    {
        var notFoundMessage = $"Shift ID {shiftId} does not exist in examination ID {examinationId}";
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId);
        var shiftGuid = ParseGuid(shiftId, notFoundMessage);

        var shift = _context.Shifts
           .Include(s => s.ShiftGroup)
           .Include(s => s.InvigilatorShift)
           .FirstOrDefault(s => s.Id == shiftGuid && s.ShiftGroup.ExaminationId == examinationGuid);

        if (shift == null)
            throw new NotFoundException(notFoundMessage);

        if (request.HandoverUserId != null)
        {
            var guid = ParseGuid(request.HandoverUserId, "User ID does not exist!");
            var userIdIsValid = shift.InvigilatorShift.Any(ivs => ivs.InvigilatorId == guid);
            if (userIdIsValid)
                shift.HandedOverUserId = guid;
            else
                throw new BadRequestException("User ID is not assigned in this shift");
        }

        if (request.Report != null)
            shift.Report = request.Report;

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Auto assign teachers to shift group
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/shift/calculate")]
    public Result<bool> AutoAssignTeachersToShiftGroup(string examinationId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.AssignInvigilator);

        var departmentShiftGroups = _context.DepartmentShiftGroups
           .Where(dg =>
                dg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid &&
                !dg.FacultyShiftGroup.ShiftGroup.DepartmentAssign)
           .Include(dg => dg.FacultyShiftGroup)
           .ThenInclude(fg => fg.ShiftGroup)
           .ThenInclude(g => g.Module)
           .Include(dg => dg.User)
           .ThenInclude(u => u!.Department)
           .OrderBy(dg => dg.FacultyShiftGroup.ShiftGroup.StartAt)
           .ThenBy(dg => dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId)
           .ToList();

        var departmentShiftGroupsDict = new Dictionary<string, List<DepartmentShiftGroup>>();
        foreach (var dg in departmentShiftGroups)
        {
            var key = dg.FacultyShiftGroup.ShiftGroup.StartAt.ToString("yy-MM-dd:hh:mm:ss") +
                      dg.FacultyShiftGroup.ShiftGroup.Module.DisplayId;
            if (!departmentShiftGroupsDict.ContainsKey(key))
                departmentShiftGroupsDict.Add(key, new List<DepartmentShiftGroup>());
            departmentShiftGroupsDict[key].Add(dg);
        }

        var invigilatorShift = _context.InvigilatorShift
           .Where(i => i.Shift.ShiftGroup.ExaminationId == examinationGuid && !i.Shift.ShiftGroup.DepartmentAssign)
           .Include(i => i.Shift)
           .ThenInclude(s => s.ShiftGroup)
           .ThenInclude(g => g.Module)
           .ToList();

        foreach (var ivs in invigilatorShift)
        {
            var shiftGroupId = ivs.Shift.ShiftGroup.StartAt.ToString("yy-MM-dd:hh:mm:ss") +
                               ivs.Shift.ShiftGroup.Module.DisplayId;
            if (!departmentShiftGroupsDict.TryGetValue(shiftGroupId, out var invigilatorsBucket))
                throw new BadRequestException(
                    $"Shift group ID does not exist: {shiftGroupId} (in invigilatorShift ID: {ivs.Id}");

            var isPrioritySlot = ivs.OrderIndex == 1;
            var priorityFacultyId = ivs.Shift.ShiftGroup.Module.FacultyId;

            for (var i = 0; i < invigilatorsBucket.Count; i++)
            {
                var departmentShiftGroup = invigilatorsBucket[i];
                var facultyOfModuleSamePriorityFaculty =
                    departmentShiftGroup.User?.Department?.FacultyId == priorityFacultyId;

                if ((isPrioritySlot && !facultyOfModuleSamePriorityFaculty) ||
                    (!isPrioritySlot && facultyOfModuleSamePriorityFaculty)) continue;

                ivs.InvigilatorId = departmentShiftGroup.UserId;
                invigilatorsBucket.RemoveAt(i);
                break;
            }
        }

        _context.SaveChanges();

        return Result<bool>.Get(true);
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

        var allTeachersInFaculty = _context.Users
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

            departmentShiftGroup.UserId = invigilator.Id;
            departmentShiftGroup.DepartmentId = invigilator.DepartmentId;
        }

        for (var i = minIndexToRandom; i < shiftGroups.Count; i++)
        {
            var departmentShiftGroup = shiftGroups[i];
            var invigilatorIndex = RandomHelper.Next(allTeachersInFaculty.Count);
            var invigilator = allTeachersInFaculty[invigilatorIndex];

            departmentShiftGroup.UserId = invigilator.Id;
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
    /// Get statistic
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns> 
    [HttpGet("{examinationId}/statistic")]
    public Result<ExaminationStatistic> GetStatistic(string examinationId)
    {
        var now = DateTime.Now;
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId);
        var startAt = _context.ShiftGroups
           .Where(g => g.ExaminationId == entity.Id)
           .Min(g => g.StartAt)
           .Date;
        var endAt = _context.ShiftGroups
           .Where(g => g.ExaminationId == entity.Id)
           .Max(g => g.StartAt)
           .AddDays(1).Date;
        var numberOfModules = _context.ShiftGroups
           .Where(g => g.ExaminationId == entity.Id)
           .GroupBy(g => g.ModuleId)
           .Count();
        var numberOfModulesOver = _context.ShiftGroups
           .Where(g => g.ExaminationId == entity.Id)
           .GroupBy(g => new
            {
                g.ModuleId,
                Over = g.StartAt < now
            })
           .Count(g => g.All(x => x.StartAt < now));
        var numberOfShifts = _context.Shifts
           .Count(s => s.ShiftGroup.ExaminationId == entity.Id);
        var numberOfShiftsOver = _context.Shifts
           .Count(s => s.ShiftGroup.StartAt < now &&
                       s.ShiftGroup.ExaminationId == entity.Id);
        var numberOfCandidates = _context.Shifts
           .Where(s => s.ShiftGroup.ExaminationId == entity.Id)
           .Sum(s => s.CandidatesCount);
        var numberOfInvigilators = _context.InvigilatorShift
           .Where(ivs =>
                ivs.DeletedAt == null &&
                ivs.InvigilatorId != null &&
                ivs.Shift.ShiftGroup.ExaminationId == entity.Id)
           .GroupBy(ivs => ivs.InvigilatorId)
           .Count();

        var result = new ExaminationStatistic
        {
            Id = entity.Id,
            DisplayId = entity.DisplayId,
            Name = entity.Name,
            StartAt = startAt,
            EndAt = endAt,
            TimePercent = endAt < now
                ? 100
                : (now - startAt) * 100f / (endAt - startAt),
            NumberOfModules = numberOfModules,
            NumberOfModulesOver = numberOfModulesOver,
            NumberOfShifts = numberOfShifts,
            NumberOfShiftsOver = numberOfShiftsOver,
            NumberOfCandidates = numberOfCandidates,
            NumberOfInvigilators = numberOfInvigilators
        };

        return Result<ExaminationStatistic>.Get(result);
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
            data = _examinationService.ValidateData(data);

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