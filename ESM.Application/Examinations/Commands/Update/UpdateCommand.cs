using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Application.Examinations.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record UpdateCommand : IRequest<Result<bool>>
{
    [FromRoute]
    public string ExaminationId { get; set; } = null!;

    public string? DisplayId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpectStartAt { get; set; }
    public DateTime? ExpectEndAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
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