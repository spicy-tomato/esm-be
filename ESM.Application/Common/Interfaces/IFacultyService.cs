using ESM.Domain.Entities;

namespace ESM.Application.Common.Interfaces;

public interface IFacultyService
{
    public Guid CheckIfExistAndReturnGuid(string facultyId);

    public Faculty CheckIfExistAndReturnEntity(string facultyId);
}