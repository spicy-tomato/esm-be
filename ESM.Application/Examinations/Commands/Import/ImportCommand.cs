using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Packaging;
using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Application.Hubs;
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
    private readonly AppHub _appHub;

    public ImportCommandHandler(IApplicationDbContext context, IExaminationService examinationService,
        AppHub appHub)
    {
        _context = context;
        _examinationService = examinationService;
        _appHub = appHub;
    }

    public async Task<Result<bool>> Handle(ImportCommand request, CancellationToken cancellationToken)
    {
        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.ExaminationId, ExaminationStatus.Idle);
        await _appHub.SendMessageImportExamination(request.ExaminationId, entity.Name, 0);

        var file = request.File;
        await _appHub.SendMessageImportExamination(request.ExaminationId, entity.Name, 25);

        List<ExaminationData> readDataResult;
        try
        {
            readDataResult = _examinationService.Import(file, request.ExaminationId);
            await _appHub.SendMessageImportExamination(request.ExaminationId, entity.Name, 50);
        }
        catch (OpenXmlPackageException)
        {
            throw new UnsupportedMediaTypeException();
        }

        await _context.ExaminationData.AddRangeAsync(readDataResult, cancellationToken);
        entity.Status = ExaminationStatus.Setup;
        await _appHub.SendMessageImportExamination(request.ExaminationId, entity.Name, 75);

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = ExaminationStatus.Setup
        });

        await _context.SaveChangesAsync(cancellationToken);
        await _appHub.SendMessageImportExamination(request.ExaminationId, entity.Name, 100);

        return Result<bool>.Get(true);
    }
}