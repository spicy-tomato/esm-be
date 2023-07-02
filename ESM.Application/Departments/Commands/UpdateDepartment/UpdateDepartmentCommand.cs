using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Departments.Exceptions;
using ESM.Application.Faculties.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Application.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommand : IRequest<Result<bool>>
{
    [FromRoute] public string DepartmentId { get; set; } = null!;

    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FacultyId { get; set; } = null!;
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.Parse(request.DepartmentId);
        var entity = await _context.Departments.FindAsync(new object[] { guid }, cancellationToken);

        if (entity == null)
        {
            throw new DepartmentNotFoundException(guid);
        }

        if (!Guid.TryParse(request.FacultyId, out var facultyGuid) ||
            _context.Faculties.FirstOrDefault(f => f.Id == facultyGuid) is null)
        {
            throw new FacultyNotFoundException(request.FacultyId);
        }

        entity.DisplayId = request.DisplayId;
        entity.Name = request.Name;
        entity.FacultyId = facultyGuid;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}