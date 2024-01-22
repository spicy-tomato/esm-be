using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Packaging;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ESM.Application.Examinations.Commands.Import;

public record ImportParams : IRequest<Result<bool>>
{
    [Required]
    public IFormFile File { get; set; } = null!;
}

public record ImportCommand(IFormFile File, string ExaminationId) : IRequest<Result<bool>>
{
    public ImportCommand(ImportParams @params, string ExaminationId) : this(@params.File, ExaminationId) { }
}

public class ImportCommandHandler : IRequestHandler<ImportCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public ImportCommandHandler(IApplicationDbContext context, IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(ImportCommand request, CancellationToken cancellationToken)
    {
        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.ExaminationId, ExaminationStatus.Idle);
        var file = request.File;

        List<ExaminationData> readDataResult;
        try
        {
            readDataResult = _examinationService.Import(file, request.ExaminationId);
        }
        catch (OpenXmlPackageException)
        {
            throw new UnsupportedMediaTypeException();
        }

        await _context.ExaminationData.AddRangeAsync(readDataResult, cancellationToken);

        entity.Status = ExaminationStatus.Setup;

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = ExaminationStatus.Setup
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }
}