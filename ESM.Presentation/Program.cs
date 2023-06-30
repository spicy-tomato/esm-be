using ESM.Application;
using ESM.Presentation;
using ESM.Presentation.Filters;
using Infrastructure;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddScoped<HttpResponseExceptionFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();

    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.UseAuthentication();

app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();