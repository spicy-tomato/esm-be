using System.Dynamic;
using ClosedXML.Excel;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Exceptions.Document;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Faculties.Exceptions;
using ESM.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Modules.Commands.Import;

public record ImportCommand : IRequest<Result<bool>>
{
    public IEnumerable<IFormFile> Files { get; init; } = Array.Empty<IFormFile>();
}

public class ImportCommandHandler : IRequestHandler<ImportCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public ImportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(ImportCommand request, CancellationToken cancellationToken)
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

        var faculties = _context.Faculties.AsNoTracking().ToList();
        var departments = _context.Departments.AsNoTracking().ToList();
        var modules = new List<Module>();

        var importResult = Import(file);
        foreach (var row in importResult)
        {
            var faculty = faculties.FirstOrDefault(f => f.Name == row.facultyName);
            if (faculty == null)
                throw new FacultyNotFoundException(row.facultyName, "name");

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

        _context.Modules.AddRange(modules);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private static IEnumerable<dynamic> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new List<dynamic>();

        var ws = wb.Worksheets.FirstOrDefault();
        if (ws == null)
        {
            throw new EmptyFileException();
        }

        var currRow = 1;
        while (!ws.Cell(currRow + 1, 1).Value.IsBlank)
        {
            currRow++;
            var moduleId = ws.Row(currRow).Cell(1).GetText();
            var moduleName = ws.Row(currRow).Cell(2).GetText();
            var credits = (int)ws.Row(currRow).Cell(3).GetDouble();
            var facultyName = ws.Row(currRow).Cell(4).GetText();

            if (!ws.Row(currRow).Cell(5).Value.TryGetText(out var departmentName))
                departmentName = "";

            dynamic row = new ExpandoObject();
            row.moduleId = moduleId;
            row.moduleName = moduleName;
            row.credits = credits;
            row.facultyName = facultyName;
            row.departmentName = departmentName;

            result.Add(row);
        }

        return result;
    }
}