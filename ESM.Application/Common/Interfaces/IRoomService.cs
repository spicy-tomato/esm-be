using Microsoft.AspNetCore.Http;

namespace ESM.Application.Common.Interfaces;

public interface IRoomService
{
    public IEnumerable<string> Import(IFormFile file);
}