using System.Net;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FacultyController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly FacultyRepository _facultyRepository;
    private readonly ModuleRepository _moduleRepository;

    #endregion

    #region Constructor

    public FacultyController(IMapper mapper,
        FacultyRepository facultyRepository,
        ApplicationContext context,
        ModuleRepository moduleRepository) :
        base(mapper)
    {
        _facultyRepository = facultyRepository;
        _context = context;
        _moduleRepository = moduleRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create faculty
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<FacultySummary?> Create(CreateFacultyRequest request)
    {
        var faculty = Validate<CreateFacultyRequest, CreateFacultyRequestValidator>(request);

        _facultyRepository.Create(faculty);
        var response = Mapper.ProjectTo<FacultySummary>(_context.Faculties)
           .FirstOrDefault(f => f.Id == faculty.Id);

        return Result<FacultySummary?>.Get(response);
    }

    /// <summary>
    /// Update faculty
    /// </summary>
    /// <param name="request"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{facultyId}")]
    public Result<FacultySummary?> Create([FromBody] UpdateFacultyRequest request, string facultyId)
    {
        const string notFoundMessage = "Faculty ID does not exist!";

        if (!Guid.TryParse(facultyId, out var guid))
            throw new NotFoundException(notFoundMessage);
        var faculty = Validate<UpdateFacultyRequest, UpdateFacultyRequestValidator>(request, guid);

        var success = _facultyRepository.Update(faculty);
        if (!success)
            throw new NotFoundException(notFoundMessage);

        var response = Mapper.ProjectTo<FacultySummary>(_context.Faculties)
           .FirstOrDefault(f => f.Id == faculty.Id);
        return Result<FacultySummary?>.Get(response);
    }

    /// <summary>
    /// Create module
    /// </summary>
    /// <param name="request"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("{facultyId}/modules")]
    public Result<ModuleSimple?> CreateModule([FromBody] CreateModuleRequest request, string facultyId)
    {
        new CreateModuleRequestValidator().ValidateAndThrow(request);
        if (!Guid.TryParse(facultyId, out var guid))
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
        var response = Mapper.ProjectTo<ModuleSimple>(_context.Modules)
           .FirstOrDefault(f => f.Id == module.Id);

        return Result<ModuleSimple?>.Get(response);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Validate and map model
    /// </summary>
    /// <param name="request"></param>
    /// <param name="facultyId"></param>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    private Faculty Validate<R, V>(R request, Guid? facultyId = null) where V : AbstractValidator<R>, new()
    {
        new V().ValidateAndThrow(request);
        var faculty = Mapper.Map<Faculty>(request,
            opts => opts.AfterMap((_, des) =>
            {
                if (facultyId != null)
                    des.Id = facultyId.Value;
            }));

        var existedFaculty = _facultyRepository.FindOne(f =>
            f.Name == faculty.Name ||
            (f.DisplayId != null && f.DisplayId == faculty.DisplayId)
        );
        if (existedFaculty == null)
            return faculty;

        var errorList = new List<Error>();
        if (existedFaculty.DisplayId == faculty.DisplayId)
            errorList.Add(new Error("displayId", "Mã khoa"));
        if (existedFaculty.Name == faculty.Name)
            errorList.Add(new Error("name", "Tên khoa"));

        throw new HttpException(HttpStatusCode.Conflict, errorList);
    }

    #endregion
}