using Microsoft.EntityFrameworkCore;
using TodoAPI;
using TodoAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();

var app = builder.Build();

// define a 'version set' that applies to an API group
var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(APIVersion.V1)
                    .ReportApiVersions()
                    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () =>
{
    return "Welcome to Derrick's Minimal API POC!";
});

app.AddTodoEndpoints(versionSet);

app.Run();


