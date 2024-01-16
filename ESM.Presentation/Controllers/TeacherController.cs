using ESM.Application.Common.Models;
using ESM.Application.Teachers.Commands.Update;
using ESM.Application.Teachers.Queries.Get;
using ESM.Application.Teachers.Queries.Search;
using ESM.Domain.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ApiControllerBase
{
    #region Public Methods

    [HttpGet(Name = "GetAllTeacher")]
    [Authorize]
    public async Task<Result<IEnumerable<GetDto>>> GetAll([FromQuery] GetQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Check if user exists or not
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("search", Name = nameof(Search))]
    [Authorize]
    public async Task<Result<IEnumerable<UserSummary>>> Search([FromQuery] SearchQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPut("{teacherId}", Name = nameof(UpdateInfo))]
    [Authorize]
    public async Task<Result<bool>> UpdateInfo(string teacherId, [FromBody] UpdateRequest request)
    {
        var command = new UpdateCommand(teacherId, request);
        return await Mediator.Send(command);
    }

    #endregion
}