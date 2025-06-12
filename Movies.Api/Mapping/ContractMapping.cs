using Microsoft.AspNetCore.Http.Features;
using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

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
    
    public static Movie MapToMovie(this UpdateMovieRequest request , Guid Id)
    {
        var movie = new Movie()
        {
            id = Id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()

        };
        return movie;
    }
    
   
    public static MovieResponse MapToResponse (this Movie movie)
    {
        return new MovieResponse()
        {
            id =  movie.id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres.ToList()

        };
       
    }
    
    public static MoviesResponse MapToResponse (this IEnumerable<Movie> movies)
    {
        return new MoviesResponse{
           Items =  movies.Select(MapToResponse)
           };
    }
}