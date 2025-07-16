using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public partial class Movie
{
    public required Guid id { get; init; }
    public required string Title { get; set; }

    public string slug => generateSlug();
    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; set; } = new();
    
    public int? userRating { get; set; }
    
    public float? Rating { get; set; }
    private string generateSlug()
    {
        var slugedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower()
            .Replace(" ", "-");

        return $"{slugedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9a-zA-Z_\\-\\s]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();


}