using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrainComponentManagement.BLL.Mapping;
using TrainComponentManagement.BLL.Validation;
using TrainComponentManagement.DAL.Data;
using TrainComponentManagement.PL.Infrastructure;
using TrainComponentManagement.PL.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrainComponentManagement API", Version = "v1" });
});

builder.Services.AddMemoryCache();

builder.Services.AddDbContextPool<TrainComponentContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
    )
);

builder.Services.RegisterBusinessLogicServices();
builder.Services.RegisterDataAccessRepositories();

builder.Services.AddAutoMapper(typeof(ComponentProfile).Assembly);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<CreateComponentDtoValidator>();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainComponentManagement API v1");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();
await app.RunAsync();
