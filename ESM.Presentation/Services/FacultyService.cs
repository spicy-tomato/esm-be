using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;
using ESM.Domain.Entities;

namespace ESM.Presentation.Services;

public class FacultyService : IFacultyService
{
    #region Properties

    private readonly IApplicationDbContext _context;

    #endregion

    public FacultyService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    #region Public methods

    public Guid CheckIfExistAndReturnGuid(string examinationId)
    {
        var guid = ParseGuid(examinationId);

        var entity = _context.Faculties
            .FirstOrDefault(f => f.Id == guid);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Faculty), guid);
        }

        return guid;
    }

    public Faculty CheckIfExistAndReturnEntity(string examinationId)
    {
        var guid = ParseGuid(examinationId);

        var entity = _context.Faculties
            .FirstOrDefault(f => f.Id == guid);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Faculty), guid);
        }

        return entity;
    }

    private static Guid ParseGuid(string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            throw new NotFoundException(nameof(Faculty), id);
        }

        return guid;
    }

    #endregion
}