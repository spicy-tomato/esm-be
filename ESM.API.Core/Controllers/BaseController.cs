using System.Security.Claims;
using AutoMapper;
using ESM.Common.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Core.API.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;

    public BaseController(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected Guid GetUserId()
    {
        var valueFromClaims = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        if (valueFromClaims == null)
            throw new UnauthorizedException();
        return Guid.Parse(valueFromClaims);
    }

    protected string GetUserName()
    {
        var userName = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
        if (userName == null)
            throw new UnauthorizedException();
        return userName;
    }
}