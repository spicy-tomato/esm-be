using System.Net;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Department;
using ESM.Data.Dtos.Faculty;
using ESM.Data.Dtos.Invigilator;
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Request.User;
using ESM.Data.Validations.Department;
using ESM.Data.Validations.User;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DepartmentController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly UserManager<User> _userManager;
    private readonly DepartmentRepository _departmentRepository;
    private readonly FacultyRepository _facultyRepository;
    private readonly UserRepository _userRepository;
    private readonly InvigilatorRepository _invigilatorRepository;
    private const string NOT_FOUND_MESSAGE = "Department ID does not exist!";

    #endregion

    #region Constructor

    public DepartmentController(IMapper mapper,
        DepartmentRepository departmentRepository,
        ApplicationContext context,
        FacultyRepository facultyRepository,
        UserRepository userRepository,
        UserManager<User> userManager,
        InvigilatorRepository invigilatorRepository) :
        base(mapper)
    {
        _departmentRepository = departmentRepository;
        _context = context;
        _facultyRepository = facultyRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _invigilatorRepository = invigilatorRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Get all departments
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Result<IEnumerable<FacultyWithDepartments>> GetAll()
    {
        var result = _facultyRepository.GetAllWithDepartments();
        return Result<IEnumerable<FacultyWithDepartments>>.Get(result);
    }

    /// <summary>
    /// Create department
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost]
    public Result<DepartmentSummary?> Create(CreateDepartmentRequest request)
    {
        var department = ValidateAndMap<CreateDepartmentRequest, CreateDepartmentRequestValidator>(request);

        _departmentRepository.Create(department);
        var response = Mapper.ProjectTo<DepartmentSummary>(_context.Departments)
           .FirstOrDefault(d => d.Id == department.Id);

        return Result<DepartmentSummary?>.Get(response);
    }

    /// <summary>
    /// Import departments and faculties
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("import")]
    public async Task<Result<bool>> Import()
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

        var importResult = DepartmentService.Import(file);
        var tasks = importResult.Select(async pair =>
        {
            var facultyName = pair.Key;
            var departmentNames = pair.Value;

            var faculty = await _facultyRepository.CreateAsync(new Faculty
                {
                    Name = facultyName
                },
                false);

            await _departmentRepository.CreateRangeAsync(departmentNames.Select(name => new Department
            {
                Name = name,
                FacultyId = faculty.Entity.Id
            }));
        });

        await Task.WhenAll(tasks);
        await _context.SaveChangesAsync();

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Update department
    /// </summary>
    /// <param name="request"></param>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{departmentId}")]
    public async Task<Result<DepartmentSummary?>> Update([FromBody] UpdateDepartmentRequest request,
        string departmentId)
    {
        var guid = ParseGuid(departmentId);
        var department = ValidateAndMap<UpdateDepartmentRequest, UpdateDepartmentRequestValidator>(request, guid);

        _departmentRepository.Update(department);

        var success = await _context.SaveChangesAsync() > 0;
        if (!success)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        var response = Mapper.ProjectTo<DepartmentSummary>(_context.Departments)
           .FirstOrDefault(f => f.Id == department.Id);
        return Result<DepartmentSummary?>.Get(response);
    }

    /// <summary>
    /// Create user for department
    /// </summary>
    /// <param name="request"></param>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ConflictException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    [HttpPost("{departmentId}/user")]
    public async Task<Result<InvigilatorSimple?>> CreateUser([FromBody] CreateUserRequest request, string departmentId)
    {
        var guid = ParseGuid(departmentId);
        await new CreateUserRequestValidator().ValidateAndThrowAsync(request);
        var user = Mapper.Map<User>(request,
            opts => opts.AfterMap((_, des) =>
            {
                des.DepartmentId = guid;
            }));
        var invigilator = Mapper.Map<Invigilator>(request);

        var existedUser = await _userRepository.FindOneAsync(u => u.Email == user.Email);
        if (existedUser != null)
            throw new ConflictException("This email has been taken");

        var existedInvigilator = await _invigilatorRepository.FindOneAsync(i =>
            invigilator.DisplayId != null && i.DisplayId == invigilator.DisplayId);
        if (existedInvigilator != null)
            throw new ConflictException("This teacher ID has been taken");

        var createUserResult = await _userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
            throw new InternalServerErrorException("Cannot create account");

        var createdUser = await _userManager.FindByEmailAsync(user.Email);
        invigilator.User = createdUser;
        var createdInvigilator = await _invigilatorRepository.CreateAsync(invigilator);

        var response = Mapper.ProjectTo<InvigilatorSimple>(_context.Invigilators)
           .FirstOrDefault(d => d.Id == createdInvigilator.Entity.Id);

        return Result<InvigilatorSimple?>.Get(response);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Validate and map model
    /// </summary>
    /// <param name="request"></param>
    /// <param name="departmentId"></param>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    /// <exception cref="HttpException"></exception>
    private Department ValidateAndMap<D, V>(D request, Guid? departmentId = null) where V : AbstractValidator<D>, new()
    {
        new V().ValidateAndThrow(request);
        var department = Mapper.Map<Department>(request,
            opts => opts.AfterMap((_, des) =>
            {
                if (departmentId != null)
                    des.Id = departmentId.Value;
            }));

        var existedDepartment = _departmentRepository.FindOne(d =>
            (departmentId == null || d.Id != departmentId) &&
            (d.Name == department.Name ||
             (d.DisplayId != null && d.DisplayId == department.DisplayId))
        );
        if (existedDepartment == null)
            return department;

        var errorList = new List<Error>();
        if (existedDepartment.DisplayId == department.DisplayId)
            errorList.Add(new Error("displayId", "Mã bộ môn"));
        if (existedDepartment.Name == department.Name)
            errorList.Add(new Error("name", "Tên bộ môn"));

        throw new HttpException(HttpStatusCode.Conflict, errorList);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    private static Guid ParseGuid(string departmentId)
    {
        if (!Guid.TryParse(departmentId, out var guid))
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        return guid;
    }

    #endregion
}