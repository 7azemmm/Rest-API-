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
            slug = movie.slug,
            Title = movie.Title,
            Rating = movie.Rating,
            userRating = movie.userRating,
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
    
    public static IEnumerable<UserRaitngsResponse> MapToResponse (this IEnumerable<MovieRatings> ratings)
    {
        return ratings.Select(x => new UserRaitngsResponse
        {
            MovieId = x.MovieId,
            slug = x.slug,
            rating = x.rating
        });
    }
    
    public static GetAllMoviesOptions MapToOptions(this GetAllMoviesRequest request ,Guid? userId)
    {
        return new GetAllMoviesOptions{
            title = request.title,
            YearOfRealease = request.Year,
            UserId = userId,
            sortOrder = request.sortBy is null ? sortOrder.unsorted : request.sortBy.StartsWith('+') ? sortOrder.descending 
                : sortOrder.ascending,
            sortField = request.sortBy?.Trim('+','-')
           
            
        };
    }

 
}