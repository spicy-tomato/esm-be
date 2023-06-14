using Microsoft.AspNetCore.Http;

namespace ESM.Application.Common.Interfaces;

public interface IModuleService
{
    public IEnumerable<dynamic> Import(IFormFile file);
}