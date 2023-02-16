using AutoMapper;
using ESM.API.Repositories.Implementations;
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

    #endregion

    #region Constructor

    public ExaminationController(IMapper mapper, ExaminationRepository examinationRepository) : base(mapper)
    {
        _examinationRepository = examinationRepository;
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