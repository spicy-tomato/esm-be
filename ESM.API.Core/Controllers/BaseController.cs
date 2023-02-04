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

    protected string GetUserId()
    {
        try
        {
            return User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
        }
        catch
        {
            throw new UnauthorizedException();
        }
    }

    protected string GetUserName()
    {
        try
        {
            return User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
        }
        catch
        {
            throw new UnauthorizedException();
        }
    }
}