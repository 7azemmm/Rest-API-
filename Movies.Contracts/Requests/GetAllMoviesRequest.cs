namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest
{
    public required string? title { get; init; }
    public required int? Year { get; init; } 
    
    public required string? sortBy { get; init; }
}