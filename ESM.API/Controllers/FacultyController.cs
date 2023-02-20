using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Dtos.Module;
using ESM.Data.Models;
using ESM.Data.Request.Faculty;
using ESM.Data.Request.Module;
using ESM.Data.Validations.Faculty;
using ESM.Data.Validations.Module;
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
    private readonly ModuleRepository _moduleRepository;

    #endregion

    #region Constructor

    public FacultyController(IMapper mapper, FacultyRepository facultyRepository, ApplicationContext context, ModuleRepository moduleRepository) :
        base(mapper)
    {
        _facultyRepository = facultyRepository;
        _context = context;
        _moduleRepository = moduleRepository;
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

    /// <summary>
    /// Create module
    /// </summary>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("{id}/modules")]
    public Result<ModuleSimple?> AddModule([FromBody] CreateModuleRequest request, string id)
    {
        new CreateModuleRequestValidator().ValidateAndThrow(request);
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException("Faculty id does not exist!");
        }

        var module = Mapper.Map<Module>(request,
            opt => opt.AfterMap((_, des) =>
            {
                des.FacultyId = guid;
            }));

        var existedModule = _moduleRepository.FindOne(m =>
            m.FacultyId == module.FacultyId && m.DisplayId == module.DisplayId);
        if (existedModule != null)
        {
            var conflictProperty = existedModule.Name == module.Name ? "name" : "id";
            throw new ConflictException($"This module {conflictProperty} has been taken");
        }

        _moduleRepository.Create(module);
        var response = Mapper.ProjectTo<ModuleSimple>(_context.Modules, null)
           .FirstOrDefault(f => f.Id == module.Id);

        return Result<ModuleSimple?>.Get(response);
    }

    #endregion
}