using DocumentFormat.OpenXml.Packaging;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Data.Enums;
using ESM.Domain.Entities;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Application.Examinations.Commands.Import;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ImportCommand : IRequest<Result<bool>>
{
    [FromRoute]
    public string ExaminationId { get; set; } = null!;

    [FromForm]
    public IFormFile? File { get; set; }

    [FromForm]
    public DateTime CreatedAt { get; set; }
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
        var file = request.File!;

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