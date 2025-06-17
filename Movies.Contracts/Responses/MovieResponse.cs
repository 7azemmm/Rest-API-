using System.Text.RegularExpressions;

namespace Movies.Contracts.Responses;

public class MovieResponse
{


    public required Guid id { get; init; }
    public required string Title { get; set; }

    public string slug { get; set; }
    public required int YearOfRelease { get; set; }
    public required IEnumerable<string> Genres { get; set; } = Enumerable.Empty<string>();
    
    public MovieResponse(){}
    public MovieResponse(Guid id, string title, int yearOfRelease , List<string> genres , string slug)
    {
        id = id;
        slug = slug;
        Title = title;
        YearOfRelease = yearOfRelease;
        Genres = genres;
    }

   
}