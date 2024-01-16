using ESM.Application.Common.Models;
using ESM.Application.Modules.Commands.Create;
using ESM.Application.Modules.Commands.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ModuleController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Create module
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost(Name = nameof(CreateModule))]
    public async Task<Result<Guid>> CreateModule([FromBody] CreateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import modules
    /// </summary>
    /// <returns></returns>
    [HttpPost("import", Name = "ImportExaminationModule")]
    public async Task<Result<bool>> Import(ImportCommand command)
    {
        return await Mediator.Send(command);
    }

    #endregion
}