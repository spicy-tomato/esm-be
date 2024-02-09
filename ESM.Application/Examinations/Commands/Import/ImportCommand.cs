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
    private readonly IPusherService _pusherService;

    public ImportCommandHandler(IApplicationDbContext context, IExaminationService examinationService,
        IPusherService pusherService)
    {
        _context = context;
        _examinationService = examinationService;
        _pusherService = pusherService;
    }

    public async Task<Result<bool>> Handle(ImportCommand request, CancellationToken cancellationToken)
    {
        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.ExaminationId, ExaminationStatus.Idle);
        await _pusherService.SendMessageImportExamination(0);

        var file = request.File;
        await _pusherService.SendMessageImportExamination(25);

        List<ExaminationData> readDataResult;
        try
        {
            readDataResult = _examinationService.Import(file, request.ExaminationId);
            await _pusherService.SendMessageImportExamination(50);
        }
        catch (OpenXmlPackageException)
        {
            throw new UnsupportedMediaTypeException();
        }

        await _context.ExaminationData.AddRangeAsync(readDataResult, cancellationToken);
        entity.Status = ExaminationStatus.Setup;
        await _pusherService.SendMessageImportExamination(75);

        _context.ExaminationEvents.Add(new ExaminationEvent
        {
            ExaminationId = entity.Id,
            Status = ExaminationStatus.Setup
        });

        await _context.SaveChangesAsync(cancellationToken);
        await _pusherService.SendMessageImportExamination(100);

        return Result<bool>.Get(true);
    }

    // private sealed class PercentageReporter
    // {
    //     private readonly IHubContext<AppHub> _appHub;
    //     private readonly string? _userId;
    //     private readonly string _examinationId;
    //     private readonly string _examinationName;
    //
    //     public PercentageReporter(IHubContext<AppHub> appHub, string? userId, string examinationId, string examinationName)
    //     {
    //         _appHub = appHub;
    //         _userId = userId;
    //         _examinationId = examinationId;
    //         _examinationName = examinationName;
    //     }
    //
    //     public async Task SendPercentageReport(int percentage)
    //     {
    //         await _appHub.SendMessageImportExamination(_userId, _examinationId, _examinationName, percentage);
    //     }
    // }
}