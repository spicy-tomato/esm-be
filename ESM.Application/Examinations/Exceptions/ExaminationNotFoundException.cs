using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Examinations.Exceptions;

public class ExaminationNotFoundException : NotFoundException
{
    public ExaminationNotFoundException(Guid id) : base(nameof(Examination), id) { }

    public ExaminationNotFoundException(string id) : base(nameof(Examination), id) { }
}