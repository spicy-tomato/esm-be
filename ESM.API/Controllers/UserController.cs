using AutoMapper;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using ESM.Data.Request.User;
using ESM.Data.Validations.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    #region Properties

    private readonly UserManager<User> _userManager;
    private readonly UserRepository _userRepository;
    private readonly JwtService _jwtService;

    #endregion

    #region Constructor

    public UserController(IMapper mapper,
        UserRepository userRepository,
        UserManager<User> userManager,
        JwtService jwtService) : base(mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    #endregion

    #region Public Methods

    [HttpPost]
    public async Task<Result<UserSummary?>> SignUp(CreateUserRequest request)
    {
        // ReSharper disable once MethodHasAsyncOverload
        new CreateUserRequestValidator().ValidateAndThrow(request);
        var user = Mapper.Map<User>(request);

        var existedUser = _userRepository.FindOne(u =>
            u.DepartmentId == user.DepartmentId && (u.UserName == user.UserName || u.DisplayId == user.DisplayId));
        if (existedUser != null)
        {
            var conflictProperty = existedUser.UserName == user.DisplayId ? "username" : "id";
            throw new ConflictException($"This user {conflictProperty} has been taken");
        }

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerErrorException("Cannot create account");
        }
        
        var response = Mapper.Map<UserSummary?>(await _userManager.FindByNameAsync(user.UserName));
        return Result<UserSummary?>.Get(response);
    }

    [HttpPost("login")]
    public async Task<Result<GeneratedToken>> Login(LoginRequest request)
    {
        // ReSharper disable once MethodHasAsyncOverload
        new LoginValidator().ValidateAndThrow(request);
        User user;

        if (request.UserName.Contains('@'))
        {
            // Email
            user = await _userManager.FindByEmailAsync(request.UserName);
        }
        else
        {
            // UserName
            user = await _userManager.FindByNameAsync(request.UserName);
        }

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException();
        }

        var token = _jwtService.CreateToken(user);
        return Result<GeneratedToken>.Get(token);
    }

    [HttpGet("summary")]
    public Result<UserSummary> GetMySummaryInfo()
    {
        var userId = GetUserId();
        var result = _userRepository.GetById(userId);

        if (result == null)
        {
            throw new NotFoundException("User does not exist!");
        }
        
        return Result<UserSummary>.Get(result);
    }
    
    #endregion
}