using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Examination;
using ESM.Data.Models;
using ESM.Data.Request.Examination;
using ESM.Data.Validations.Examination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExaminationController : BaseController
{
    #region Properties

    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationDataRepository _examinationDataRepository;
    private readonly ExaminationService _examinationService;

    #endregion

    #region Constructor

    public ExaminationController(IMapper mapper,
        ExaminationRepository examinationRepository,
        ExaminationDataRepository examinationDataRepository,
        ExaminationService examinationService) : base(mapper)
    {
        _examinationRepository = examinationRepository;
        _examinationDataRepository = examinationDataRepository;
        _examinationService = examinationService;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create new examination
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns> 
    [HttpPost]
    [Authorize]
    public Result<ExaminationSummary> Create(CreateExaminationRequest request)
    {
        new CreateExaminationRequestValidator().ValidateAndThrow(request);
        var examination = Mapper.Map<Examination>(request);
        examination.IsActive = true;
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
    [Authorize]
    public Result<IEnumerable<ExaminationSummary>> GetRelated()
    {
        var filterActive = Request.Query["isActive"].ToString() == "true";
        var userId = GetUserId();

        var createdExamination =
            _examinationRepository.Find(e => e.CreatedBy.Id == userId && (!filterActive || e.IsActive));

        return Result<IEnumerable<ExaminationSummary>>.Get(createdExamination);
    }

    /// <summary>
    /// Get data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns> 
    [HttpGet("{id}")]
    [Authorize]
    public async Task<Result<IEnumerable<ExaminationData>>> GetData(string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException("Examination does not exist!");
        }

        var data = _examinationDataRepository.Find(e => e.ExaminationId == guid);
        data = await _examinationService.ValidateData(data);

        return Result<IEnumerable<ExaminationData>>.Get(data);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id}")]
    [Authorize]
    public Result<bool> Import(string id)
    {
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
            readDataResult = ExaminationService.Import(file, id);
        }
        catch (OpenXmlPackageException)
        {
            throw new UnsupportedMediaTypeException();
        }

        _examinationDataRepository.CreateRange(readDataResult);

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Get summary
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns> 
    [HttpGet("{id}/summary")]
    [Authorize]
    public Result<ExaminationSummary> GetSummary(string id)
    {
        const string notFoundMessage = "Examination does not exist!";
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException(notFoundMessage);
        }

        var createdExamination = _examinationRepository.GetById(guid);
        if (createdExamination == null)
        {
            throw new NotFoundException(notFoundMessage);
        }

        return Result<ExaminationSummary>.Get(createdExamination);
    }

    #endregion
}