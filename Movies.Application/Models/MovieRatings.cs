namespace Movies.Application.Models;

public class MovieRatings
{
    public required Guid MovieId { get; set; }
    public required string slug { get; set; }
    public required int rating { get; set; }
}
