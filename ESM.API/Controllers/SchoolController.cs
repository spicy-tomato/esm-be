using AutoMapper;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Faculty;
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
    private readonly FacultyRepository _facultyRepository;

    #endregion

    #region Constructor

    public SchoolController(IMapper mapper,
        SchoolRepository schoolRepository,
        FacultyRepository facultyRepository) :
        base(mapper)
    {
        _schoolRepository = schoolRepository;
        _facultyRepository = facultyRepository;
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

    /// <summary>
    /// Get departments in school
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpGet("{id}/departments")]
    public Result<IEnumerable<FacultyWithDepartments>> GetDepartments(string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException("School ID does not exist!");
        }

        var result = _facultyRepository.Find(f => f.SchoolId == guid);

        return Result<IEnumerable<FacultyWithDepartments>>.Get(result);
    }

    #endregion
}