using AgendaPro.Api.Extensions;
using AgendaPro.Application.Tags.UseCase;
using AgendaPro.Domain.Tags.Repositories;
using AgendaPro.Infrastucture.Tags;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AgendaPro.Infrastucture;
using AgendaPro.Application.Services.UseCases;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Infrastucture.Services; // Add the correct using directive for ServiceRepository

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// adiciona configuracao da camada de infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddApplicationSwagger();

builder.Services.AddScoped<TagUseCase>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ServiceUseCase>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseValidationFilter>();
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseApplicationSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
