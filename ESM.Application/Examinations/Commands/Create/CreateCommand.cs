using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Examinations.Commands.Create;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CreateCommand : IRequest<Result<Guid>>
{
    public string? DisplayId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? ExpectStartAt { get; set; }
    public DateTime? ExpectEndAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCommandHandler : IRequestHandler<CreateCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.UserId ?? "", out var currentUserId))
        {
            throw new UnauthorizedException();
        }

        var entity = new Examination
        {
            DisplayId = request.DisplayId,
            Name = request.Name,
            Description = request.Description,
            ExpectStartAt = request.ExpectStartAt,
            ExpectEndAt = request.ExpectEndAt,
            CreatedAt = request.CreatedAt,
            Status = ExaminationStatus.Idle,
            CreatedBy = currentUserId
        };

        _context.Examinations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(entity.Id);
    }
}