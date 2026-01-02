namespace GameStore.Api.Dtos;

public record NewGameDTO
(
    string Title,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string Developer,
    string Publisher
);
