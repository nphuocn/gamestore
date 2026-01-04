namespace GameStore.Api.Dtos;

public record GameDetailDTO
(
    int Id,
    string Title,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate,
    string? Developer,
    string Publisher
);