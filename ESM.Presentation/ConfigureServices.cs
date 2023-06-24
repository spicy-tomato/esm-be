using ESM.API.Services;
using ESM.Application.Common.Interfaces;
using ESM.Presentation.Filters;
using ESM.Presentation.Services;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;

namespace ESM.Presentation;

public static class ConfigureServices
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IExaminationService, ExaminationService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddHttpContextAccessor();
        
        services.AddMvc(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            })
           .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        
        services.AddFluentValidationAutoValidation(conf =>
            {
                conf.DisableDataAnnotationsValidation = true;
            })
           .AddFluentValidationClientsideAdapters();
    }
}