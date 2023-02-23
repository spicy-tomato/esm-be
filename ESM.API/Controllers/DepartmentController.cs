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
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Validations.Department;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DepartmentController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly DepartmentRepository _departmentRepository;
    private readonly FacultyRepository _facultyRepository;

    #endregion

    #region Constructor

    public DepartmentController(IMapper mapper,
        DepartmentRepository departmentRepository,
        ApplicationContext context,
        FacultyRepository facultyRepository) :
        base(mapper)
    {
        _departmentRepository = departmentRepository;
        _context = context;
        _facultyRepository = facultyRepository;
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
        const string notFoundMessage = "Department ID does not exist!";

        if (!Guid.TryParse(departmentId, out var guid))
            throw new NotFoundException(notFoundMessage);
        var department = ValidateAndMap<UpdateDepartmentRequest, UpdateDepartmentRequestValidator>(request, guid);

        _departmentRepository.Update(department);

        var success = await _context.SaveChangesAsync() > 0;
        if (!success)
            throw new NotFoundException(notFoundMessage);

        var response = Mapper.ProjectTo<DepartmentSummary>(_context.Departments)
           .FirstOrDefault(f => f.Id == department.Id);
        return Result<DepartmentSummary?>.Get(response);
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
}