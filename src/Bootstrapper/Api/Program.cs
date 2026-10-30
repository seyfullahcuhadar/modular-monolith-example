using Basket.Infrastructure;
using Catalog.Infrastructure;
using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = false);

builder.Services.AddApplication(
    Catalog.Application.AssemblyReference.Assembly,
    Basket.Application.AssemblyReference.Assembly);
builder.Services.AddInfrastructure();

builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddBasketModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.ApplyModuleMigrationsAsync();

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEndpoints();

await app.RunAsync();
