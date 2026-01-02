using GameStore.Api.Endpoints;
using GameStore.Api.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<NewGameDTOValidator>();

var app = builder.Build();

app.MapGameStoreEndpoints();

app.Run();