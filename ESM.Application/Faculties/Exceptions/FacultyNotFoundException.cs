using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Faculties.Exceptions;

public class FacultyNotFoundException : NotFoundException
{
    public FacultyNotFoundException(string fieldValue, string fieldName = "ID")
        : base(nameof(Faculty), fieldValue, fieldName) { }

    public FacultyNotFoundException(Guid id) : base(nameof(Faculty), id) { }
}