using System.Text;
using ESM.API.Contexts;
using ESM.API.Repositories.Implementations;
using ESM.API.Services;
using ESM.Core.API.Filters;
using ESM.Data.Models;
using ESM.Domain.Mappings;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        options.User.RequireUniqueEmail = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
   .AddRoles<IdentityRole<Guid>>()
   .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ExaminationService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<ExaminationDataRepository>();
builder.Services.AddScoped<ExaminationRepository>();
builder.Services.AddScoped<FacultyRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<RoomRepository>();
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
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
