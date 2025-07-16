namespace Movies.Contracts.Responses;

public class UserRaitngsResponse
{
    public required Guid MovieId { get; set; }
    public required string slug { get; set; }
    public required int rating { get; set; }
}