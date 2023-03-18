using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
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
    public Result<ExaminationSummary> Create(CreateExaminationRequest request)
    {
        new CreateExaminationRequestValidator().ValidateAndThrow(request);
        var examination = Mapper.Map<Examination>(request);
        examination.Status = ExaminationStatus.Idle;
        examination.CreatedById = GetUserId();

        var createdExamination = _examinationRepository.Create(examination);
        var response = Mapper.Map<ExaminationSummary>(createdExamination);

        return Result<ExaminationSummary>.Get(response);
    }

    /// <summary>
    /// Get related examinations of current user
    /// </summary>
    /// <returns></returns>
    [HttpGet("related")]
    public Result<List<GetRelatedResponseItem>> GetRelated([FromQuery] bool? filterActive)
    {
        var createdExamination =
            Mapper.ProjectTo<GetRelatedResponseItem>(
                    _context.Examinations
                       .Where(e => filterActive == null || !filterActive.Value || e.Status > 0)
                       .OrderBy(e => e.CreatedAt)
                )
               .ToList();

        return Result<List<GetRelatedResponseItem>>.Get(createdExamination);
    }

    /// <summary>
    /// Get examination data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="departmentAssign"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}")]
    public Result<List<GetDataResponseItem>> GetData(string examinationId,
        [FromQuery] bool? departmentAssign)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId,
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator);

        var data = Mapper.ProjectTo<GetDataResponseItem>(
                _context.Shifts
                   .Where(
                        e => e.ShiftGroup.ExaminationId == guid &&
                             (departmentAssign == null || e.ShiftGroup.DepartmentAssign == departmentAssign)
                    )
                   .OrderBy(s => s.ShiftGroup.StartAt)
                   .ThenBy(s => s.ShiftGroup.Id)
                   .ThenBy(s => s.ShiftGroup.Module.Name)
                   .ThenBy(s => s.Room.DisplayId)
            )
           .ToList();

        return Result<List<GetDataResponseItem>>.Get(data);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("{examinationId}")]
    public Result<bool> Import(string examinationId)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.Idle);

        IFormFile file;

        try
        {
            file = Request.Form.Files[0];
        }
        catch (Exception)
        {
            throw new UnsupportedMediaTypeException();
        }

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
        _context.SaveChanges();

        return Result<bool>.Get(true);
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
                   .Include(s => s.ShiftGroup)
                   .ThenInclude(s => s.FacultyShiftGroups)
                   .ThenInclude(fg => fg.DepartmentShiftGroups)
                   .Where(g => g.ShiftGroup.ExaminationId == examinationGuid && !g.ShiftGroup.DepartmentAssign)
                   .OrderBy(g => g.ShiftGroup.StartAt)
                   .ThenBy(g => g.ShiftGroup.ModuleId)
                   .ThenBy(g => g.Room.Id)
            ).ToList();
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
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.AssignInvigilator);

        _context.Entry(entity)
           .Collection(e => e.ShiftGroups)
           .Query()
           .Include(eg => eg.Shifts)
           .ThenInclude(fg => fg.InvigilatorShift)
           .Load();

        foreach (var shiftGroup in entity.ShiftGroups)
        foreach (var shift in shiftGroup.Shifts)
        foreach (var invigilatorShift in shift.InvigilatorShift)
            if (request.TryGetValue(invigilatorShift.Id.ToString(), out var invigilatorId))
            {
                if (Guid.TryParse(invigilatorId, out var invigilatorGuid))
                    invigilatorShift.InvigilatorId = invigilatorGuid;
                else
                    throw new BadRequestException($"Cannot parse invigilator ID to Guid: {invigilatorId}");
            }


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

                if ((!isPrioritySlot || !facultyOfModuleSamePriorityFaculty) &&
                    (isPrioritySlot || facultyOfModuleSamePriorityFaculty)) continue;

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
    /// <param name="newStatus"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpPost("{examinationId}/status")]
    public Result<bool> ChangeStatus([FromBody] ExaminationStatus newStatus, string examinationId)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId);

        switch (entity.Status, newStatus)
        {
            case (ExaminationStatus.Setup, ExaminationStatus.AssignFaculty):
                FinishSetup(entity);
                break;
            case (ExaminationStatus.AssignFaculty, ExaminationStatus.AssignInvigilator):
                FinishAssignFaculty(entity);
                break;
            // case (ExaminationStatus.AssignInvigilator):
            //     break;
            default:
                var expectedOldStatus = (ExaminationStatus)((int)newStatus - 1);
                if (Enum.IsDefined(typeof(ExaminationStatus), expectedOldStatus))
                    throw new BadRequestException($"Examination status should be {expectedOldStatus}");
                throw new BadRequestException("New status is invalid");
        }

        _context.SaveChanges();

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

        foreach (var (shiftGroupId, examsCount) in request)
        {
            if (!Guid.TryParse(shiftGroupId, out var shiftGuid))
                throw new NotFoundException(notFoundMessage + shiftGroupId);

            var found = false;
            foreach (var shiftGroup in entity.ShiftGroups)
            {
                foreach (var shift in shiftGroup.Shifts)
                {
                    if (shiftGroup.Id != shiftGuid)
                        continue;

                    shift.ExamsCount = examsCount;
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new NotFoundException(notFoundMessage + shiftGroupId);
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
    public Result<List<GetGroupByFacultyIdResponseItem>> GetGroupsByFacultyId(string examinationId, string facultyId)
    {
        var examinationGuid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.AssignInvigilator);
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
        ).ToList();

        return Result<List<GetGroupByFacultyIdResponseItem>>.Get(data);
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
        var facultyShiftGroupsQuery = _context.DepartmentShiftGroups
           .Where(fg =>
                fg.FacultyShiftGroup.FacultyId == facultyGuid &&
                fg.FacultyShiftGroup.ShiftGroup.ExaminationId == examinationGuid)
           .Include(dg => dg.FacultyShiftGroup)
           .ThenInclude(fg => fg.ShiftGroup);
        var facultyShiftGroups = facultyShiftGroupsQuery.ToDictionary(fg => fg.Id.ToString(), fg => fg);

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
        var rand = new Random();

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
            var invigilatorIndex = rand.Next(allTeachersInFaculty.Count);
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
            ExaminationStatus.AssignFaculty | ExaminationStatus.AssignInvigilator);
        var data = Mapper.ProjectTo<GetAllGroupsResponseResponseItem>(
            _context.ShiftGroups
               .Include(eg => eg.FacultyShiftGroups)
               .Where(e => e.ExaminationId == guid && !e.DepartmentAssign)
               .OrderBy(eg => eg.StartAt)
        ).ToList();
        var facultyShiftGroup = _context.FacultyShiftGroups
           .Where(fg => fg.ShiftGroup.ExaminationId == guid && !fg.ShiftGroup.DepartmentAssign)
           .ToList();
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

            foreach (var faculty in faculties)
            {
                var calculatedInvigilatorsCount = faculty.Id == mainFacultyId
                    ? group.RoomsCount
                    : Convert.ToInt32((group.InvigilatorsCount - group.RoomsCount) *
                                      (teachersNumberInFaculties.GetValueOrDefault(faculty.Id, 0) * 1.0 /
                                       teachersNumberInRestFaculties));
                var savedRecord = group.FacultyShiftGroups
                   .FirstOrDefault(feg => feg.FacultyId == faculty.Id);
                if (savedRecord == null)
                    group.FacultyShiftGroups.Add(new FacultyShiftGroup
                    {
                        FacultyId = faculty.Id,
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
                )
               .ToList();

        var priorityFacultyOfShiftGroupsQuery = _context.ShiftGroups
           .Where(g => g.ExaminationId == guid)
           .Include(g => g.Module)
           .Select(g => new { g.Id, g.Module.FacultyId })
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
                    if (departmentShiftGroup.User == null)
                        continue;

                    var isPriority = departmentShiftGroup.User.Department?.FacultyId == priorityFacultyId;
                    list.Add(new GetAvailableInvigilatorsInShiftGroup.ResponseItem
                    {
                        Id = departmentShiftGroup.User.Id,
                        FullName = departmentShiftGroup.User.FullName,
                        InvigilatorId = departmentShiftGroup.User.InvigilatorId,
                        IsPriority = isPriority
                    });
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
    public Result<List<ExaminationData>> GetTemporaryData(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Setup);

        var data = GetTemporaryData(guid);
        return Result<List<ExaminationData>>.Get(data);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Get temporary data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="skipValidate"></param>
    /// <returns></returns>
    private List<ExaminationData> GetTemporaryData(Guid examinationId, bool skipValidate = false)
    {
        var data = _examinationDataRepository.Find(e => e.ExaminationId == examinationId).ToList();
        if (!skipValidate && data.Count > 0)
            data = _examinationService.ValidateData(data);

        return data;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private static Guid ParseGuid(string facultyId)
    {
        if (!Guid.TryParse(facultyId, out var guid))
            throw new NotFoundException(NOT_FOUND_MESSAGE);
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

    #endregion
}