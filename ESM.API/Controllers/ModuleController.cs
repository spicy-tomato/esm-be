using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos.Room;
using ESM.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ModuleController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly FacultyRepository _facultyRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly ModuleRepository _moduleRepository;

    #endregion

    #region Constructor

    public ModuleController(IMapper mapper,
        ApplicationContext context,
        FacultyRepository facultyRepository,
        DepartmentRepository departmentRepository,
        ModuleRepository moduleRepository) :
        base(mapper)
    {
        _context = context;
        _facultyRepository = facultyRepository;
        _departmentRepository = departmentRepository;
        _moduleRepository = moduleRepository;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Import modules
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("import")]
    public Result<bool> Import()
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

        var faculties = _facultyRepository.GetAll().ToList();
        var departments = _departmentRepository.GetAll().ToList();
        var modules = new List<Module>();

        var importResult = ModuleService.Import(file);
        foreach (var row in importResult)
        {
            var faculty = faculties.FirstOrDefault(f => f.Name == row.facultyName);
            if (faculty == null)
                throw new NotFoundException($"Faculty name does not exists: {row.facultyName}");

            Department? department = null;
            if (!string.IsNullOrEmpty(row.departmentName))
                department = departments.FirstOrDefault(f => f.Name == row.departmentName);

            var facultyId = faculty.Id;

            modules.Add(new Module
            {
                DisplayId = row.moduleId,
                Name = row.moduleName,
                Credits = row.credits,
                FacultyId = facultyId,
                DepartmentId = department?.Id
            });
        }

        _moduleRepository.CreateRange(modules);
        _context.SaveChanges();

        return Result<RoomSummary?>.Get(true);
    }

    #endregion
}