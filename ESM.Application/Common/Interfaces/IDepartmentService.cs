using Microsoft.AspNetCore.Http;

namespace ESM.Application.Common.Interfaces;

public interface IDepartmentService
{
    public Dictionary<string, Dictionary<string, List<KeyValuePair<string, string>>>> Import(IFormFile file);

    public Guid CheckIfExistAndReturnGuid(string id);
}