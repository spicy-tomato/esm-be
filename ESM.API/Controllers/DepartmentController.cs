using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Department;
using ESM.Data.Models;
using ESM.Data.Request.Department;
using ESM.Data.Validations.Department;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly DepartmentRepository _departmentRepository;

    #endregion

    #region Constructor

    public DepartmentController(IMapper mapper, DepartmentRepository departmentRepository, ApplicationContext context) :
        base(mapper)
    {
        _departmentRepository = departmentRepository;
        _context = context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add new department
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Result<DepartmentSummary?> Create(CreateDepartmentRequest request)
    {
        new CreateDepartmentRequestValidator().ValidateAndThrow(request);
        var department = Mapper.Map<Department>(request);

        var existedDepartment = _departmentRepository.FindOne(f =>
            f.SchoolId == department.SchoolId &&
            f.FacultyId == department.FacultyId &&
            (f.Name == department.Name || f.DisplayId == department.DisplayId));
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

    #endregion
}