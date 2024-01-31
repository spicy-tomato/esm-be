using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;
using ESM.Application.Departments.Commands.CreateDepartment;
using ESM.Application.Departments.Commands.CreateUserInDepartment;
using ESM.Application.Departments.Commands.ImportDepartment;
using ESM.Application.Departments.Commands.UpdateDepartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DepartmentController : ApiControllerBase
{
    /// <summary>
    /// Create department
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="ConflictException"></exception>
    [HttpPost(Name = "CreateDepartment")]
    public async Task<Result<Guid>> Create(CreateDepartmentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import departments and faculties
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("import", Name = "ImportDepartment")]
    public async Task<Result<bool>> Import([FromForm] ImportDepartmentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update department
    /// </summary>
    /// <param name="params"></param>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{departmentId}", Name = "UpdateDepartment")]
    public async Task<Result<bool>> Update(UpdateDepartmentParams @params, string departmentId)
    {
        var command = new UpdateDepartmentCommand(@params, departmentId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Create user for department
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ConflictException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    [HttpPost("{departmentId}/user", Name = nameof(CreateUser))]
    public async Task<Result<Guid>> CreateUser(CreateUserInDepartmentParams @params, string departmentId)
    {
        var command = new CreateUserInDepartmentCommand(@params, departmentId);
        return await Mediator.Send(command);
    }
}