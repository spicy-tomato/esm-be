using AutoMapper;
using ESM.API.Repositories.Implementations;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Models;
using ESM.Data.Request.School;
using ESM.Data.Validations.School;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolController : BaseController
{
    #region Properties

    private readonly SchoolRepository _schoolRepository;

    #endregion

    #region Constructor

    public SchoolController(IMapper mapper, SchoolRepository schoolRepository) : base(mapper)
    {
        _schoolRepository = schoolRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add new school
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<bool> Create(CreateSchoolRequest request)
    {
        new CreateSchoolRequestValidator().ValidateAndThrow(request);
        var school = Mapper.Map<School>(request);

        _schoolRepository.Create(school);

        return Result<bool>.Get(true);
    }

    #endregion
}