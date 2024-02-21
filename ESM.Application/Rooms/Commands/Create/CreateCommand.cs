using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Rooms.Exceptions;
using ESM.Domain.Entities;
using MediatR;

namespace ESM.Application.Rooms.Commands.Create;

public record CreateCommand(string DisplayId, int? Capacity) : IRequest<Result<Guid>>;

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
            throw new ExistedRootException();
        }

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Get(room.Id);
    }
}