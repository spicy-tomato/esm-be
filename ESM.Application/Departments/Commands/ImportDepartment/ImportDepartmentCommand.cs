using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Helpers;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ESM.Application.Departments.Commands.ImportDepartment;

public class ImportDepartmentCommand : IRequest<Result<bool>>
{
    public IEnumerable<IFormFile> Files { get; set; } = Array.Empty<IFormFile>();
}

public class ImportDepartmentCommandHandler : IRequestHandler<ImportDepartmentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDepartmentService _departmentService;
    private readonly IIdentityService _identityService;

    public ImportDepartmentCommandHandler(IApplicationDbContext context,
        IDepartmentService departmentService,
        IIdentityService identityService)
    {
        _context = context;
        _departmentService = departmentService;
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(ImportDepartmentCommand request, CancellationToken cancellationToken)
    {
        IFormFile file;
        try
        {
            file = request.Files.ToArray()[0];
        }
        catch (Exception)
        {
            throw new UnsupportedMediaTypeException();
        }

        var importResult = _departmentService.Import(file);

        foreach (var (facultyName, departments) in importResult)
        {
            EntityEntry<Faculty>? faculty = null;
            if (facultyName != "Khác")
            {
                await AddFacultyAndAccountsInFaculty(facultyName);
            }

            foreach (var (departmentName, teachers) in departments)
            {
                var departmentEntity = new Department
                {
                    Name = departmentName,
                    FacultyId = faculty?.Entity.Id
                };

                _context.Departments.Add(departmentEntity);

                foreach (var teacher in teachers)
                {
                    await AddTeacherAccountToDepartment(teacher, departmentEntity);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private async Task AddFacultyAndAccountsInFaculty(string facultyName)
    {
        var abbreviation = string.Join("", facultyName.Split(' ')
            .Select(c => c[0])
            .Where(c => c != '-')
        ).ToUpper();

        var facultyEntity = new Faculty
        {
            Name = facultyName,
            DisplayId = abbreviation
        };

        _context.Faculties.Add(facultyEntity);
        await _context.SaveChangesAsync();

        var createUserResponse = await _identityService.CreateUserAsync("K_" + abbreviation,
            StringHelper.RandomEmail());
        var addToRoleResponse = await _identityService.AddUserToRoleAsync(createUserResponse.UserId, "Teacher");

        if (createUserResponse.Result.Success && addToRoleResponse.Success)
        {
            var teacher = new Teacher
            {
                UserId = createUserResponse.UserId,
                FullName = "Khoa " + abbreviation,
                FacultyId = facultyEntity.Id
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
        }
    }

    private async Task AddTeacherAccountToDepartment(KeyValuePair<string, string> teacher, Department departmentEntity)
    {
        var createUserResponse = await _identityService.CreateUserAsync("GV" + teacher.Key,
            StringHelper.RandomEmail());
        var addToRoleResponse = await _identityService.AddUserToRoleAsync(createUserResponse.UserId, "Teacher");

        if (createUserResponse.Result.Success && addToRoleResponse.Success)
        {
            var teacherEntity = new Teacher
            {
                UserId = createUserResponse.UserId,
                FullName = teacher.Value,
                Department = departmentEntity,
                TeacherId = teacher.Key
            };

            _context.Teachers.Add(teacherEntity);
        }
    }
}