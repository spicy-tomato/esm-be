using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
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
    /// Add new department
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost]
    public Result<DepartmentSummary?> Create(CreateDepartmentRequest request)
    {
        new CreateDepartmentRequestValidator().ValidateAndThrow(request);
        var department = Mapper.Map<Department>(request);

        var existedDepartment = _departmentRepository.FindOne(f =>
            f.Name == department.Name ||
            (f.DisplayId != null && f.DisplayId == department.DisplayId)
        );
        if (existedDepartment != null)
        {
            var conflictProperty = existedDepartment.Name == department.Name ? "name" : "id";
            throw new ConflictException($"This department {conflictProperty} has been taken");
        }

        _departmentRepository.Create(department);
        var response = Mapper.ProjectTo<DepartmentSummary>(_context.Departments, null)
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

    #endregion
}