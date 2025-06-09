using Movies.Application.Models;
using Movies.Contracts.Requests;

namespace RestApi.Mapping;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        var movie = new Movie()
        {
            id =  Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()

        };
        return movie;
    }
    
}