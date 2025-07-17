namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public required string? title { get; init; }
    public required int? YearOfRealease { get; init; } 
    public required Guid? UserId { get; init; }  =default;
}