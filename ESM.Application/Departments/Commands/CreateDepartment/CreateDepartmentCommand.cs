using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using MediatR;

namespace ESM.Application.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand : IRequest<Result<Guid>>
{
    public string? DisplayId { get; init; }
    public string Name { get; init; } = null!;
    public Guid? FacultyId { get; init; }
}

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            DisplayId = request.DisplayId,
            FacultyId = request.FacultyId,
            Name = request.Name
        };

        _context.Departments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(entity.Id);
    }
}