using GameStore.Api.Endpoints;
using GameStore.Api.Validators;
using FluentValidation;
using GameStore.Api.Data;
using Microsoft.Extensions.Options;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<NewGameDTOValidator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddOriginCORS();

// // Add GameStore database context
builder.AddGameStoreDb();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();

// // Apply database migrations
app.MigrateDb();

// Map GameStore endpoints can not run 
// while adding swagger causes errors related to https redirection
// app.MapGameStoreEndpoints();

app.Run();