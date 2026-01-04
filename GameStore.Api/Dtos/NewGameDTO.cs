namespace GameStore.Api.Dtos;

public record NewGameDTO
(
    string Title,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate,
    string Developer,
    string Publisher
);
