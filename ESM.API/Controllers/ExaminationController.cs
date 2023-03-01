using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Common.Core.Helpers;
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
    private readonly ModuleRepository _moduleRepository;
    private readonly RoomRepository _roomRepository;

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
        ModuleRepository moduleRepository,
        RoomRepository roomRepository) : base(mapper)
    {
        _examinationRepository = examinationRepository;
        _examinationDataRepository = examinationDataRepository;
        _examinationService = examinationService;
        _context = context;
        _examinationShiftRepository = examinationShiftRepository;
        _moduleRepository = moduleRepository;
        _roomRepository = roomRepository;
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
        var departmentAssignQuery = Request.Query["departmentAssign"].ToString();
        bool? departmentAssign =
            departmentAssignQuery switch
            {
                "true" => true,
                "false" => false,
                _ => null
            };

        var guid = ParseGuid(examinationId);
        var data = _examinationShiftRepository.Find(
            e => e.ExaminationId == guid &&
                 (departmentAssign == null || e.DepartmentAssign == departmentAssign)
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
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations.FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Idle)
            throw new BadRequestException("Examination status should be ExaminationStatus.Idle");

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
        var guid = ParseGuid(examinationId);
        var entity = _context.Examinations.FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Setup)
            throw new BadRequestException("Examination status should be ExaminationStatus.Setup");

        var modules = _moduleRepository.GetAll();
        var modulesDictionary = modules.ToDictionary(m => m.DisplayId, m => m.Id);

        var rooms = _roomRepository.GetAll();
        var roomsDictionary = rooms.ToDictionary(m => m.DisplayId, m => m.Id);

        var examinationShifts = new List<ExaminationShift>();
        var data = await GetTemporaryData(guid, true);

        foreach (var shift in data)
        {
            var roomsInShift = RoomHelper.GetRoomsFromString(shift.Rooms);
            foreach (var room in roomsInShift)
            {
                examinationShifts.Add(new ExaminationShift
                {
                    Method = shift.Method!.Value,
                    ExamsCount = ExaminationHelper.CalculateExamsNumber(shift),
                    CandidatesCount = shift.CandidatesCount!.Value,
                    InvigilatorsCount = ExaminationHelper.CalculateInvigilatorNumber(shift),
                    StartAt = shift.StartAt!.Value,
                    Shift = shift.Shift,
                    ExaminationId = guid,
                    ModuleId = modulesDictionary[shift.ModuleId!],
                    RoomId = roomsDictionary[room],
                    DepartmentAssign = shift.DepartmentAssign ?? false
                });
            }
        }

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
           .Include(e => e.ExaminationsShift)
           .FirstOrDefault(e => e.Id == guid);
        if (entity == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        if (entity.Status != ExaminationStatus.Active)
            throw new BadRequestException("Examination status should be ExaminationStatus.Active");

        const string notFoundMessage = "Shift ID does not exists: ";

        foreach (var row in request)
        {
            if (!Guid.TryParse(row.Key, out var shiftGuid))
                throw new NotFoundException(notFoundMessage + row.Key);

            var found = false;
            foreach (var examinationShift in entity.ExaminationsShift)
            {
                if (examinationShift.Id != shiftGuid) 
                    continue;

                examinationShift.ExamsCount = row.Value;
                found = true;
                break;
            }
            
            if (!found)
                throw new NotFoundException(notFoundMessage + row.Key);
        }

        _context.SaveChanges();

        return Task.FromResult(Result<bool>.Get(true));
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
        var guid = ParseGuid(examinationId);
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

    #endregion
}