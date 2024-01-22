using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;
using ESM.Application.Common.Exceptions.Document;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ESM.Application.Rooms.Commands.Import;

public record ImportCommand : IRequest<Result<bool>>
{
    [Required]
    public IFormFile File { get; init; } = null!;
}

public class ImportCommandHandler : IRequestHandler<ImportCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public ImportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(ImportCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;
        var rooms = Import(file);

        _context.Rooms.AddRange(rooms);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private static IEnumerable<Room> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var roomsName = new List<string>();

        var ws = wb.Worksheets.FirstOrDefault();
        if (ws is null)
        {
            throw new EmptyFileException();
        }

        var currRow = 0;
        while (!ws.Cell(currRow + 1, 1).Value.IsBlank)
        {
            currRow++;
            var room = ws.Row(currRow).Cell(1).GetText();
            roomsName.Add(room);
        }

        var rooms = roomsName.Select(r => new Room
        {
            DisplayId = r
        });

        return rooms;
    }
}