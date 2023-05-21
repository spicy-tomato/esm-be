using System.Net;
using AutoMapper;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Common.Core.Exceptions;
using ESM.Core.API.Controllers;
using ESM.Data.Core.Response;
using ESM.Data.Dtos;
using ESM.Data.Dtos.User;
using ESM.Data.Models;
using ESM.Data.Params.User;
using ESM.Data.Request.User;
using ESM.Data.Validations.User;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESM.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    #region Properties

    private readonly ApplicationContext _context;
    private readonly UserManager<User> _userManager;
    private readonly UserRepository _userRepository;
    private readonly JwtService _jwtService;
    private const string NOT_FOUND_MESSAGE = "User ID does not exist!";

    #endregion

    #region Constructor

    public UserController(IMapper mapper,
        UserRepository userRepository,
        UserManager<User> userManager,
        JwtService jwtService,
        ApplicationContext context) : base(mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
    }

    #endregion

    #region Public Methods

    [HttpGet]
    [Authorize]
    public Result<IEnumerable<UserSummary>> GetUsers([FromQuery] bool? isInvigilator)
    {
        var result = _userRepository.Find(u =>
            isInvigilator == null || isInvigilator == false || u.InvigilatorId != null
        );

        return Result<IEnumerable<UserSummary>>.Get(result);
    }

    // [HttpPost]
    // public async Task<Result<UserSummary?>> SignUp(CreateUserRequest request)
    // {
    //     // ReSharper disable once MethodHasAsyncOverload
    //     new CreateUserRequestValidator().ValidateAndThrow(request);
    //     var user = Mapper.Map<User>(request);
    //
    //     var existedUser =
    //         _userRepository.FindOne(u => u.DepartmentId == user.DepartmentId && u.UserName == user.UserName);
    //     if (existedUser != null)
    //     {
    //         throw new ConflictException("This user username has been taken");
    //     }
    //
    //     var result = await _userManager.CreateAsync(user, request.Password);
    //     if (!result.Succeeded)
    //     {
    //         throw new InternalServerErrorException("Cannot create account");
    //     }
    //
    //     var response = Mapper.Map<UserSummary?>(await _userManager.FindByNameAsync(user.UserName));
    //     return Result<UserSummary?>.Get(response);
    // }

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPatch("change-password")]
    [Authorize]
    public async Task<Result<bool>> ChangePassword([FromBody] ChangePasswordRequest query)
    {
        var userId = GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new UnauthorizedException(NOT_FOUND_MESSAGE);

        await _userManager.ChangePasswordAsync(user, query.Password, query.NewPassword);

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Check if user exists or not
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("check-exist")]
    [Authorize]
    public Result<bool> CheckIfExist([FromQuery] CheckIfExistParams query)
    {
        var userIsExist = _context.Users
           .FirstOrDefault(u =>
                (query.InvigilatorId == null || u.InvigilatorId == query.InvigilatorId) &&
                (query.Email == null || u.InvigilatorId == query.Email)
            ) != null;
        return Result<bool>.Get(userIsExist);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    [HttpPost("login")]
    public async Task<Result<GeneratedToken>> Login(LoginRequest request)
    {
        // ReSharper disable once MethodHasAsyncOverload
        new LoginValidator().ValidateAndThrow(request);
        User user;

        if (request.UserName.Contains('@'))
            // Email
            user = await _userManager.FindByEmailAsync(request.UserName);
        else
            // UserName
            user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedException();

        var token = _jwtService.CreateToken(user);
        return Result<GeneratedToken>.Get(token);
    }

    /// <summary>
    /// Check if user exists or not
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("search")]
    [Authorize]
    public Result<IQueryable<UserSummary>> Search([FromQuery] SearchParams query)
    {
        var result = Mapper.ProjectTo<UserSummary>(
            _context.Users
               .Where(u => query.FullName == null || EF.Functions.Like(u.FullName, $"%{query.FullName}%"))
               .Take(20)
        );
        return Result<IQueryable<UserSummary>>.Get(result);
    }

    /// <summary>
    /// Get user summary
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    [HttpGet("summary")]
    [Authorize]
    public Result<UserSummary> GetMySummaryInfo()
    {
        var userId = GetUserId();
        var result = _userRepository.GetById(userId);

        if (result == null)
            throw new UnauthorizedException(NOT_FOUND_MESSAGE);

        return Result<UserSummary>.Get(result);
    }

    [HttpGet("{userId}")]
    [Authorize]
    public Result<UserSummary> GetUserById(string userId)
    {
        var guid = ParseGuid(userId);
        var result = _userRepository.GetById(guid);
        if (result == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        return Result<UserSummary>.Get(result);
    }


    [HttpPut("{userId}")]
    [Authorize]
    public async Task<Result<UserSummary>> UpdateInfo([FromBody] UpdateUserRequest request, string userId)
    {
        var guid = ParseGuid(userId);
        var departmentGuid = Guid.Empty;
        if (request.DepartmentId != null && !Guid.TryParse(request.DepartmentId, out departmentGuid))
            throw new NotFoundException("Faculty ID does not exist!");

        await new UpdateUserRequestValidator().ValidateAndThrowAsync(request);

        var userWithIdFromRequest = _context.Users.FirstOrDefault(u => u.Id == guid);
        if (userWithIdFromRequest == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        var errorList = _userRepository.GetDuplicatedDataErrors(guid, request.Email, request.InvigilatorId);
        if (errorList.Count > 0)
            throw new HttpException(HttpStatusCode.Conflict, errorList);

        userWithIdFromRequest.InvigilatorId = request.InvigilatorId;
        userWithIdFromRequest.FullName = request.FullName;
        userWithIdFromRequest.IsMale = request.IsMale;
        if (departmentGuid != Guid.Empty)
            userWithIdFromRequest.DepartmentId = departmentGuid;

        await _userManager.SetEmailAsync(userWithIdFromRequest, request.Email);
        await _context.SaveChangesAsync();

        var result = _userRepository.GetById(guid);
        if (result == null)
            throw new NotFoundException(NOT_FOUND_MESSAGE);

        return Result<UserSummary>.Get(result);
    }

    #endregion

    #region Private methods

    private static Guid ParseGuid(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        return guid;
    }

    #endregion
}