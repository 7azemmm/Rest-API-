using Movies.Contracts.Responses;

namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest : PageRequest
{
    public required string? title { get; init; }
    public required int? Year { get; init; } 
    
    public required string? sortBy { get; init; }
}