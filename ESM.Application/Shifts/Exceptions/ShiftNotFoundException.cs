using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Shifts.Exceptions;

public class ShiftNotFoundException : NotFoundException
{
    public ShiftNotFoundException(string roleName) : base(nameof(Shift), roleName) { }
}