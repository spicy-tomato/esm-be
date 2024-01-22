using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Departments.Exceptions;

public class DepartmentNotFoundException : NotFoundException
{
    public DepartmentNotFoundException(Guid id) : base(nameof(Department), id) { }

    public DepartmentNotFoundException(string id) : base(nameof(Department), id) { }
}