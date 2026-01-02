using FluentValidation;
using GameStore.Api.Dtos;

namespace GameStore.Api.Validators;

public class NewGameDTOValidator : AbstractValidator<NewGameDTO>
{
    public NewGameDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .Must(g => new[] { "RPG", "Action", "Action-Adventure", "Sandbox", "Strategy" }.Contains(g))
            .WithMessage("Genre must be one of: RPG, Action, Action-Adventure, Sandbox, Strategy");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThanOrEqualTo(199m).WithMessage("Price cannot exceed $199");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Release date cannot be in the future");

        RuleFor(x => x.Developer)
            .NotEmpty().WithMessage("Developer is required")
            .Length(1, 100).WithMessage("Developer name must be between 1 and 100 characters");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher is required")
            .Length(1, 100).WithMessage("Publisher name must be between 1 and 100 characters");
    }
}
