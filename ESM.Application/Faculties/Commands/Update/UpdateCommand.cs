using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Faculties.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateCommand(string Id, UpdateRequest Request) : IRequest<Result<bool>>;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFacultyService _facultyService;

    public UpdateCommandHandler(IApplicationDbContext context, IFacultyService facultyService)
    {
        _context = context;
        _facultyService = facultyService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var faculty = _facultyService.CheckIfExistAndReturnEntity(request.Id);

        faculty.DisplayId = request.Request.DisplayId;
        faculty.Name = request.Request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}