using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Examinations.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateParams(
    string? DisplayId,
    string? Name,
    string? Description,
    DateTime? ExpectStartAt,
    DateTime? ExpectEndAt,
    DateTime? UpdatedAt);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateCommand(
    string? DisplayId,
    string? Name,
    string? Description,
    DateTime? ExpectStartAt,
    DateTime? ExpectEndAt,
    DateTime? UpdatedAt,
    string ExaminationId) : IRequest<Result<bool>>
{
    public UpdateCommand(UpdateParams @params, string ExaminationId) : this(
        @params.DisplayId,
        @params.Name,
        @params.Description,
        @params.ExpectStartAt,
        @params.ExpectEndAt,
        @params.UpdatedAt,
        ExaminationId
    ) { }
}

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public UpdateCommandHandler(IApplicationDbContext context, IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _examinationService.CheckIfExaminationExistAndReturnEntity(request.ExaminationId);

        entity.DisplayId = request.DisplayId ?? entity.DisplayId;
        entity.Name = request.Name ?? entity.Name;
        entity.Description = request.Description ?? entity.Description;
        entity.ExpectStartAt = request.ExpectStartAt ?? entity.ExpectStartAt;
        entity.ExpectEndAt = request.ExpectEndAt ?? entity.ExpectEndAt;
        entity.UpdatedAt = request.UpdatedAt;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}