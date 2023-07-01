using AutoMapper;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Dtos.Faculty;
using ESM.Domain.Entities;
using JetBrains.Annotations;
using MediatR;

namespace ESM.Application.Faculties.Commands.Create;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record CreateCommand(string? DisplayId, string Name) : IRequest<Result<FacultySummary?>>;

public class CreateCommandHandler : IRequestHandler<CreateCommand, Result<FacultySummary?>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<Result<FacultySummary?>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.UserId ?? "", out var currentUserId))
        {
            throw new BadRequestException("Cannot get current user");
        }

        var entity = new Faculty
        {
            DisplayId = request.DisplayId,
            Name = request.Name,
            CreatedBy = currentUserId
        };

        _context.Faculties.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.ProjectTo<FacultySummary>(_context.Faculties)
            .FirstOrDefault(f => f.Id == entity.Id);

        return Result<FacultySummary?>.Get(response);
    }
}