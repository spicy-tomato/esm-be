using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Rooms.Commands.Create;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CreateCommand(string DisplayId, int Capacity) : IRequest<Result<Guid>>;

public class CreateCommandHandler : IRequestHandler<CreateCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            DisplayId = request.DisplayId.Trim(),
            Capacity = request.Capacity
        };

        var existedRoom = _context.Rooms.FirstOrDefault(r => r.DisplayId == room.DisplayId);
        if (existedRoom is not null)
        {
            throw new ConflictException("This room has been existed!");
        }

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(room.Id);
    }
}