using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Modules.Exceptions;
using ESM.Domain.Entities;
using MediatR;

namespace ESM.Application.Modules.Commands.Create;

public record CreateCommand(string DisplayId, string Name, string FacultyId, string? DepartmentId)
    : IRequest<Result<Guid>>;

public class CreateCommandHandler : IRequestHandler<CreateCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDepartmentService _departmentService;
    private readonly IFacultyService _facultyService;

    public CreateCommandHandler(IApplicationDbContext context,
        IDepartmentService departmentService,
        IFacultyService facultyService)
    {
        _context = context;
        _departmentService = departmentService;
        _facultyService = facultyService;
    }

    public async Task<Result<Guid>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var facultyId = _facultyService.CheckIfExistAndReturnGuid(request.FacultyId);
        Guid? departmentId = request.DepartmentId != null
            ? _departmentService.CheckIfExistAndReturnGuid(request.DepartmentId)
            : null;

        var module = new Module
        {
            DisplayId = request.DisplayId,
            Name = request.Name,
            FacultyId = facultyId,
            DepartmentId = departmentId
        };

        var existedModule = _context.Modules
            .FirstOrDefault(m =>
                m.FacultyId == module.FacultyId &&
                m.DisplayId == module.DisplayId);

        if (existedModule is not null)
        {
            var conflictProperty = existedModule.Name == module.Name ? "name" : "id";
            throw new ConflictModuleDataException(conflictProperty);
        }

        _context.Modules.Add(module);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(module.Id);
    }
}