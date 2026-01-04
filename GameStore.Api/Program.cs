using GameStore.Api.Endpoints;
using GameStore.Api.Validators;
using FluentValidation;
using GameStore.Api.Data;
using Microsoft.Extensions.Options;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<NewGameDTOValidator>();

// Add GameStore database context
builder.AddGameStoreDb();

var app = builder.Build();

// Apply database migrations
app.MigrateDb();

app.MapGameStoreEndpoints();

app.Run();