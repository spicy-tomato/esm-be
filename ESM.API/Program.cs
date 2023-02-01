using System.Text;
using ESM.API.Contexts;
using ESM.API.Services;
using ESM.Core.API.Filters;
using ESM.Data.Models;
using ESM.Domain.Mappings;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAutoMapper(typeof(ModelMapping));

builder.Services.AddIdentityCore<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<HttpResponseExceptionFilter>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddMvc(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    })
   .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services
   .AddFluentValidationAutoValidation(conf =>
    {
        conf.DisableDataAnnotationsValidation = true;
    })
   .AddFluentValidationClientsideAdapters();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

app.UseAuthentication();

app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();