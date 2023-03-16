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
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using ESM.Data.Request.Faculty;
using ESM.Data.Request.Module;
using ESM.Data.Responses.Faculty;
using ESM.Data.Validations.Faculty;
using ESM.Data.Validations.Module;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    private readonly UserRepository _userRepository;
    private const string NOT_FOUND_MESSAGE = "Faculty ID does not exist!";

    #endregion

    #region Constructor

    public FacultyController(IMapper mapper,
        FacultyRepository facultyRepository,
        ApplicationContext context,
        ModuleRepository moduleRepository,
        UserRepository userRepository) :
        base(mapper)
    {
        _facultyRepository = facultyRepository;
        _context = context;
        _moduleRepository = moduleRepository;
        _userRepository = userRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Get all faculties
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Result<List<GetAllResponseItem>> GetAll()
    {
        var result = Mapper.ProjectTo<GetAllResponseItem>(
                _context.Faculties
                   .Include(f => f.Departments))
           .ToList();
        return Result<List<GetAllResponseItem>>.Get(result);
    }

    /// <summary>
    /// Create faculty
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<FacultySummary?> Create(CreateFacultyRequest request)
    {
        var faculty = ValidateAndMap<CreateFacultyRequest, CreateFacultyRequestValidator>(request);

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
    public Result<FacultySummary?> Update([FromBody] UpdateFacultyRequest request, string facultyId)
    {
        var guid = ParseGuid(facultyId);
        var faculty = ValidateAndMap<UpdateFacultyRequest, UpdateFacultyRequestValidator>(request, guid);

        _facultyRepository.Update(faculty);

        var success = _context.SaveChanges() > 0;
        if (!success)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        var response = Mapper.ProjectTo<FacultySummary>(_context.Faculties)
           .FirstOrDefault(f => f.Id == faculty.Id);
        return Result<FacultySummary?>.Get(response);
    }

    /// <summary>
    /// Get all users in faculty
    /// </summary>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpGet("{facultyId}/user")]
    public Result<IEnumerable<UserSummary>> GetUser(string facultyId)
    {
        var guid = ParseGuid(facultyId);
        var response = _userRepository.Find(u =>
            u.Department != null &&
            u.Department.FacultyId == guid
        );
        return Result<IEnumerable<UserSummary>>.Get(response);
    }

    /// <summary>
    /// Create module
    /// </summary>
    /// <param name="request"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("{facultyId}/module")]
    public Result<ModuleSimple?> CreateModule([FromBody] CreateModuleRequest request, string facultyId)
    {
        new CreateModuleRequestValidator().ValidateAndThrow(request);
        var guid = ParseGuid(facultyId);

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
    /// <typeparam name="R">Request</typeparam>
    /// <typeparam name="V">Request's validation</typeparam>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    private Faculty ValidateAndMap<R, V>(R request, Guid? facultyId = null) where V : AbstractValidator<R>, new()
    {
        new V().ValidateAndThrow(request);
        var faculty = Mapper.Map<Faculty>(request,
            opts => opts.AfterMap((_, des) =>
            {
                if (facultyId != null)
                    des.Id = facultyId.Value;
            }));

        var existedFaculty = _facultyRepository.FindOne(f =>
            (facultyId == null || f.Id != facultyId) &&
            (f.Name == faculty.Name ||
             (f.DisplayId != null && f.DisplayId == faculty.DisplayId))
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