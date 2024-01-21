using ESM.Application;
using ESM.Infrastructure;
using ESM.Infrastructure.Persistence;
using ESM.Presentation;
using ESM.Presentation.Filters;
using Swashbuckle.Swagger;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddScoped<HttpResponseExceptionFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(i => i.FriendlyId(true));
    options.SupportNonNullableReferenceTypes();
    options.UseOneOfForPolymorphism();
    options.SchemaFilter<SwaggerRequiredSchemaFilter>();
    options.AddEnumsWithValuesFixFilters(opt =>
    {
        opt.ApplySchemaFilter = true;
        opt.XEnumNamesAlias = "x-enum-varnames";
        opt.XEnumDescriptionsAlias = "x-enum-descriptions";
    });
});

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}

app.UseAuthentication();

app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();