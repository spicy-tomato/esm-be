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
using ESM.Data.Dtos.User;
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
    private const string NOT_FOUND_MESSAGE = "Department ID does not exist!";

    #endregion

    #region Constructor

    public DepartmentController(IMapper mapper,
        DepartmentRepository departmentRepository,
        ApplicationContext context,
        FacultyRepository facultyRepository,
        UserRepository userRepository,
        UserManager<User> userManager) :
        base(mapper)
    {
        _departmentRepository = departmentRepository;
        _context = context;
        _facultyRepository = facultyRepository;
        _userRepository = userRepository;
        _userManager = userManager;
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
        var random = new Random();

        foreach (var (facultyName, departments) in importResult)
        {
            var faculty = await _facultyRepository.CreateAsync(new Faculty
                {
                    Name = facultyName
                },
                false);

            foreach (var (departmentName, teachersName) in departments)
            {
                var department = await _departmentRepository.CreateAsync(new Department
                    {
                        Name = departmentName,
                        FacultyId = faculty.Entity.Id
                    },
                    false);

                foreach (var teacherName in teachersName)
                {
                    await _userManager.CreateAsync(new User
                    {
                        Email = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                           .Select(s => s[random.Next(s.Length)]).ToArray()) + "@com",
                        UserName = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                           .Select(s => s[random.Next(s.Length)]).ToArray()),
                        FullName = teacherName,
                        Department = department.Entity,
                        InvigilatorId = "GV" + new string(Enumerable.Repeat("0123456789", 10)
                           .Select(s => s[random.Next(s.Length)]).ToArray())
                    });
                }
            }
        }

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
    public async Task<Result<UserSummary>> CreateUser([FromBody] CreateUserRequest request, string departmentId)
    {
        var guid = ParseGuid(departmentId);
        await new CreateUserRequestValidator().ValidateAndThrowAsync(request);
        var user = Mapper.Map<User>(request,
            opts => opts.AfterMap((_, des) =>
            {
                des.DepartmentId = guid;
            }));

        var errorList = _userRepository.GetDuplicatedDataErrors(guid, request.Email, request.InvigilatorId);
        if (errorList.Count > 0)
            throw new HttpException(HttpStatusCode.Conflict, errorList);

        var createUserResult = await _userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
            throw new InternalServerErrorException("Cannot create account");

        await _context.SaveChangesAsync();

        var createdUser = await _userManager.FindByEmailAsync(request.Email);
        var response = Mapper.Map<UserSummary>(createdUser);

        return Result<UserSummary>.Get(response);
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