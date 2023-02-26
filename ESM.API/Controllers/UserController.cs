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
using ESM.Data.Request.User;
using ESM.Data.Validations.User;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public Result<IEnumerable<UserSummary>> GetAll()
    {
        var isInvigilator = Request.Query["isInvigilator"].ToString() == "true";
        var result = isInvigilator
            ? _userRepository.Find(u => u.InvigilatorId != null)
            : _userRepository.GetAll();

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

    private static Guid ParseGuid(string departmentId)
    {
        if (!Guid.TryParse(departmentId, out var guid))
            throw new NotFoundException(NOT_FOUND_MESSAGE);
        return guid;
    }

    #endregion
}