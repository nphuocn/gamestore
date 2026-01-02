namespace GameStore.Api.Dtos;

public record GameDTO
(
    int Id,
    string Title,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string Developer,
    string Publisher
);