using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Models;
using ESM.Data.Request.Faculty;
using ESM.Data.Validations.Faculty;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FacultyController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly FacultyRepository _facultyRepository;

    #endregion

    #region Constructor

    public FacultyController(IMapper mapper, FacultyRepository facultyRepository, ApplicationContext context) :
        base(mapper)
    {
        _facultyRepository = facultyRepository;
        _context = context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add new faculty
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<FacultySummary?> Create(CreateFacultyRequest request)
    {
        new CreateFacultyRequestValidator().ValidateAndThrow(request);
        var faculty = Mapper.Map<Faculty>(request);

        var existedFaculty = _facultyRepository.FindOne(f =>
            f.SchoolId == faculty.SchoolId && (f.Name == faculty.Name || f.DisplayId == faculty.DisplayId));
        if (existedFaculty != null)
        {
            var conflictProperty = existedFaculty.Name == faculty.Name ? "name" : "id";
            throw new ConflictException($"This faculty {conflictProperty} has been taken");
        }

        _facultyRepository.Create(faculty);
        var response = Mapper.ProjectTo<FacultySummary>(_context.Faculties, null)
           .FirstOrDefault(f => f.Id == faculty.Id);

        return Result<FacultySummary?>.Get(response);
    }

    #endregion
}