using ESM.Application.Common.Exceptions;
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
    [HttpPost]
    public async Task<Result<Guid>> Create(CreateDepartmentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import departments and faculties
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("import")]
    public async Task<Result<bool>> Import(ImportDepartmentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update department
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{departmentId}")]
    public async Task<Result<bool>> Update(UpdateDepartmentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Create user for department
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ConflictException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    [HttpPost("{departmentId}/user")]
    public async Task<Result<Guid>> CreateUser(CreateUserInDepartmentCommand command, string departmentId)
    {
        return await Mediator.Send(command);
    }
}