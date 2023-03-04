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
using ESM.Data.Models;
using ESM.Data.Request.Examination;
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
    private readonly ExaminationShiftRepository _examinationShiftRepository;
    private readonly ExaminationShiftGroupRepository _examinationShiftGroupRepository;
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
        ExaminationShiftRepository examinationShiftRepository,
        ExaminationShiftGroupRepository examinationShiftGroupRepository,
        FacultyRepository facultyRepository,
        UserRepository userRepository) : base(mapper)
    {
        _examinationRepository = examinationRepository;
        _examinationDataRepository = examinationDataRepository;
        _examinationService = examinationService;
        _context = context;
        _examinationShiftRepository = examinationShiftRepository;
        _examinationShiftGroupRepository = examinationShiftGroupRepository;
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
    public Result<IEnumerable<ExaminationSummary>> GetRelated()
    {
        var filterActive = Request.Query["isActive"].ToString() == "true";
        var userId = GetUserId();

        var createdExamination =
            _examinationRepository.Find(e => e.CreatedBy.Id == userId && (!filterActive || e.Status > 0));

        return Result<IEnumerable<ExaminationSummary>>.Get(createdExamination);
    }

    /// <summary>
    /// Get temporary data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}")]
    public Result<IEnumerable<ExaminationShiftSimple>> GetData(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Active);

        var departmentAssignQuery = Request.Query["departmentAssign"].ToString();
        bool? departmentAssign =
            departmentAssignQuery switch
            {
                "true" => true,
                "false" => false,
                _ => null
            };

        var data = _examinationShiftRepository.Find(
            e => e.ExaminationShiftGroup.ExaminationId == guid &&
                 (departmentAssign == null || e.ExaminationShiftGroup.DepartmentAssign == departmentAssign)
        );
        return Result<IEnumerable<ExaminationShiftSimple>>.Get(data);
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
    /// Activate examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/activate")]
    public async Task<Result<bool>> Activate(string examinationId)
    {
        var entity = CheckIfExaminationExistAndReturnEntity(examinationId, ExaminationStatus.Setup);

        var temporaryData = await GetTemporaryData(entity.Id, true);
        var examinationShifts =
            _examinationService.RetrieveExaminationShiftsFromTemporaryData(entity.Id, temporaryData);

        await _examinationShiftRepository.CreateRangeAsync(examinationShifts);
        entity.Status = ExaminationStatus.Active;
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
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations
           .Include(e => e.ExaminationsShiftGroups)
           .ThenInclude(eg => eg.ExaminationShifts)
           .FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Active)
            throw new BadRequestException("Examination status should be ExaminationStatus.Active");

        const string notFoundMessage = "Shift ID does not exists: ";

        foreach (var (shiftGroupId, examsCount) in request)
        {
            if (!Guid.TryParse(shiftGroupId, out var shiftGuid))
                throw new NotFoundException(notFoundMessage + shiftGroupId);

            var found = false;
            foreach (var examinationShiftGroup in entity.ExaminationsShiftGroups)
            {
                foreach (var examinationShift in examinationShiftGroup.ExaminationShifts)
                {
                    if (examinationShiftGroup.Id != shiftGuid)
                        continue;

                    examinationShift.ExamsCount = examsCount;
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
    /// Get all shift groups in examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/group")]
    public Result<List<ExaminationShiftGroupSimple>> GetAllGroups(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Active);
        var data = _examinationShiftGroupRepository.Find(e => e.ExaminationId == guid && !e.DepartmentAssign).ToList();
        var invigilatorsNumberInFaculties = _userRepository.CountByFaculties();

        foreach (var group in data)
        {
            CalculateShiftInvigilatorsNumber(group, invigilatorsNumberInFaculties);
        }

        return Result<List<ExaminationShiftGroupSimple>>.Get(data);
    }

    /// <summary>
    /// Calculate invigilators number of shift for each faculty
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/group")]
    public Result<bool> CalculateInvigilatorNumerateOfShiftForEachFaculty(string examinationId)
    {
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations
           .Include(e => e.ExaminationsShiftGroups)
           .ThenInclude(eg => eg.Module)
           .Include(e => e.ExaminationsShiftGroups)
           .ThenInclude(eg => eg.FacultyExaminationShiftGroups)
           .FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Active)
            throw new BadRequestException("Examination status should be ExaminationStatus.Active");

        var faculties = _facultyRepository.GetAll().ToList();
        var facultiesPercent = CalculatePercent();

        foreach (var group in entity.ExaminationsShiftGroups)
        {
            var mainFacultyId = group.Module.FacultyId;

            foreach (var faculty in faculties)
            {
                var calculatedInvigilatorsCount = faculty.Id == mainFacultyId
                    ? group.RoomsCount
                    : Convert.ToInt32((group.InvigilatorsCount - group.RoomsCount) *
                                      facultiesPercent.GetValueOrDefault(faculty.Id, 0));
                var savedRecord = group.FacultyExaminationShiftGroups
                   .FirstOrDefault(feg => feg.FacultyId == faculty.Id);
                if (savedRecord == null)
                    group.FacultyExaminationShiftGroups.Add(new FacultyExaminationShiftGroup
                    {
                        FacultyId = faculty.Id,
                        ExaminationShiftGroup = group,
                        InvigilatorsCount = calculatedInvigilatorsCount,
                        CalculatedInvigilatorsCount = calculatedInvigilatorsCount
                    });
                else
                    savedRecord.InvigilatorsCount = calculatedInvigilatorsCount;
            }
        }

        _context.SaveChanges();

        return Result<bool>.Get(true);
    }

    /// <summary>
    ///  Get shift group by id
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpGet("{examinationId}/group/{groupId}")]
    public Result<ExaminationShiftGroupSimple?> GetGroupById(string examinationId, string groupId)
    {
        if (!Guid.TryParse(groupId, out var groupGuid))
            throw new NotFoundException("Group ID does not exist!");
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Active);

        var data = _examinationShiftGroupRepository.FindOne(e =>
            e.ExaminationId == guid && !e.DepartmentAssign && e.Id == groupGuid
        );
        var invigilatorsNumberInFaculties = _userRepository.CountByFaculties();

        if (data != null)
            CalculateShiftInvigilatorsNumber(data, invigilatorsNumberInFaculties);

        return Result<ExaminationShiftGroupSimple?>.Get(data);
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
    public Result<ExaminationShiftGroupSimple?> AssignInvigilatorNumerateOfShiftToFaculty(
        [FromBody] int numberOfInvigilator,
        string examinationId,
        string groupId,
        string facultyId)
    {
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations
           .Include(e => e.ExaminationsShiftGroups)
           .ThenInclude(eg => eg.FacultyExaminationShiftGroups)
           .Select(e => new { e.Id, e.Status, e.ExaminationsShiftGroups })
           .FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Active)
            throw new BadRequestException("Examination status should be ExaminationStatus.Active");

        const string notFoundGroupMessage = "Group ID does not exist!";
        const string notFoundFacultyMessage = "Faculty ID does not exist!";

        if (!Guid.TryParse(groupId, out var groupGuid))
            throw new NotFoundException(notFoundGroupMessage);
        var group = entity.ExaminationsShiftGroups.FirstOrDefault(eg => eg.Id == groupGuid);
        if (group == null)
            throw new NotFoundException(notFoundGroupMessage);

        if (!Guid.TryParse(facultyId, out var facultyGuid))
            throw new NotFoundException(notFoundFacultyMessage);
        var facultyGroup = group.FacultyExaminationShiftGroups.FirstOrDefault(eg => eg.FacultyId == facultyGuid);
        if (facultyGroup == null)
            throw new NotFoundException(notFoundFacultyMessage);

        facultyGroup.InvigilatorsCount = numberOfInvigilator;

        _context.SaveChanges();

        var result = _examinationShiftGroupRepository.FindOne(e =>
            e.ExaminationId == guid && !e.DepartmentAssign && e.Id == groupGuid
        );
        var invigilatorsNumberInFaculties = _userRepository.CountByFaculties();

        if (result != null)
            CalculateShiftInvigilatorsNumber(result, invigilatorsNumberInFaculties);

        return Result<ExaminationShiftGroupSimple?>.Get(result);
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
    public async Task<Result<List<ExaminationData>>> GetTemporaryData(string examinationId)
    {
        var guid = CheckIfExaminationExistAndReturnGuid(examinationId, ExaminationStatus.Setup);

        var data = await GetTemporaryData(guid);
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
    private async Task<List<ExaminationData>> GetTemporaryData(Guid examinationId, bool skipValidate = false)
    {
        var data = await _examinationDataRepository.FindAsync(e => e.ExaminationId == examinationId);
        if (!skipValidate && data.Count > 0)
            data = await _examinationService.ValidateData(data);

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
        if (acceptStatus != null && status != acceptStatus)
            throw new BadRequestException($"Examination status should be ExaminationStatus.{acceptStatus.ToString()}");
        return guid;
    }

    private Examination CheckIfExaminationExistAndReturnEntity(string examinationId,
        ExaminationStatus? acceptStatus = null)
    {
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations.FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (acceptStatus != null && entity.Status != acceptStatus)
            throw new BadRequestException($"Examination status should be ExaminationStatus.{acceptStatus.ToString()}");
        return entity;
    }

    private Dictionary<Guid, double> CalculatePercent()
    {
        var teachers = _userRepository.Find(u => u.DepartmentId != null).ToList();
        var facultyTeachersCount = new Dictionary<Guid, double>();

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

        foreach (var (facultyId, teachersCount) in facultyTeachersCount)
        {
            facultyTeachersCount[facultyId] = teachersCount / teachers.Count;
        }

        return facultyTeachersCount;
    }

    private static void CalculateShiftInvigilatorsNumber(ExaminationShiftGroupSimple group,
        IReadOnlyDictionary<Guid, int> invigilatorsNumberInFaculties)
    {
        group.AssignNumerate = group.FacultyExaminationShiftGroups
           .ToDictionary(
                fg => fg.FacultyId.ToString(),
                fg => new ExaminationGroupDataCell
                {
                    Actual = fg.InvigilatorsCount,
                    Calculated = fg.CalculatedInvigilatorsCount,
                    Maximum = invigilatorsNumberInFaculties.GetValueOrDefault(fg.FacultyId, 0)
                }
            );

        var total = group.FacultyExaminationShiftGroups.Sum(feg => feg.InvigilatorsCount);
        group.AssignNumerate.Add("total",
            new ExaminationGroupDataCell
            {
                Actual = total,
                Calculated = total - group.InvigilatorsCount
            });
    }

    #endregion
}