using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;
using ESM.Application.Faculties.Commands.Create;
using ESM.Application.Faculties.Commands.Update;
using ESM.Application.Faculties.Queries.GetAll;
using ESM.Application.Faculties.Queries.GetTeachers;
using ESM.Domain.Dtos.Faculty;
using ESM.Domain.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FacultyController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Get all faculties
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetAllFaculty")]
    public async Task<Result<List<GetAllDto>>> GetAll()
    {
        var query = new GetAllQuery();
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Create faculty
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost(Name = "CreateFaculty")]
    public async Task<Result<FacultySummary?>> Create(CreateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update faculty
    /// </summary>
    /// <param name="request"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{facultyId}", Name = "UpdateFaculty")]
    public async Task<Result<bool>> Update(string facultyId, [FromBody] UpdateRequest request)
    {
        var query = new UpdateCommand(facultyId, request);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get all users in faculty
    /// </summary>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpGet("{facultyId}/users", Name = nameof(GetUser))]
    public async Task<Result<List<UserSummary>>> GetUser(string facultyId)
    {
        var query = new GetTeachersQuery(facultyId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Create module
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost("{facultyId}/module", Name = "CreateModuleFaculty")]
    [Obsolete]
    public Result<bool> CreateModule(string facultyId, object key)
    {
        // Moved to /module

        return Result<bool>.Get(true);
    }

    #endregion
}