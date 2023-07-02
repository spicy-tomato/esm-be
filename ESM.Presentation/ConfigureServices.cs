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
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IExaminationService, ExaminationService>();
        services.AddScoped<IFacultyService, FacultyService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<ITeacherService, TeacherService>();
        
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