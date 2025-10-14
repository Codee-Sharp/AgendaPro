using AgendaPro.Api.Extensions;
using AgendaPro.Application.Tags.UseCase;
using AgendaPro.Domain.Tags.Repositories;
using AgendaPro.Infrastucture.Tags;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AgendaProDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddApplicationSwagger();

builder.Services.AddScoped<TagUseCase>();
builder.Services.AddScoped<ITagRepository, TagRepository>();



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
