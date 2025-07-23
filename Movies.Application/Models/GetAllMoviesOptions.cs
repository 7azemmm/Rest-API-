namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public required string? title { get; init; }
    public required int? YearOfRealease { get; init; } 
    public required Guid? UserId { get; init; }  =default;
    public required string? sortField { get; init; }
    
    public sortOrder? sortOrder { get; init; }
}

public enum sortOrder
{
    unsorted,
    ascending,
    descending
}