using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Validations;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movievalidator;
    private readonly IRatingRepository _ratingRepository;
    public MovieService(IMovieRepository movieRepository , IValidator<Movie> validator , IRatingRepository ratingRepository)
    {
        _movieRepository = movieRepository;
        _movievalidator = validator;
        _ratingRepository = ratingRepository;
    }
    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken =default , Guid? userId = default)
    {
        return _movieRepository.GetAllAsync(cancellationToken , userId);
    }

    public Task<Movie?> GetByIdAsync(Guid id , CancellationToken cancellationToken =default , Guid? userId = default)
    {
        return _movieRepository.GetByIdAsync(id , cancellationToken , userId);
    }

    public Task<Movie?> GetBySlugAsync(string slug , CancellationToken cancellationToken =default , Guid? userId = default)
    {
        return _movieRepository.GetBySlugAsync(slug , cancellationToken , userId);
    }

    public async Task<bool> CreateAsync(Movie movie , CancellationToken cancellationToken =default)
    {
         await _movievalidator.ValidateAndThrowAsync(movie);
        return await _movieRepository.CreateAsync(movie , cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie , Guid? userId=default ,  CancellationToken cancellationToken =default)
    {
        await _movievalidator.ValidateAndThrowAsync(movie);
        var MovieExist = await _movieRepository.ExistbyIdAsync(movie.id , cancellationToken);
        if (!MovieExist)
        {
            return null;
        }
         await _movieRepository.UpdateAsync(movie , cancellationToken);
         if (!userId.HasValue)
         {
             var rating = await _ratingRepository.getRatingAsync(movie.id, cancellationToken);
             movie.Rating = rating;
             return movie;
         }
        
         var ratings = await _ratingRepository.getRatingAsync(movie.id, userId.Value, cancellationToken);
         movie.Rating = ratings.Rating;
         movie.userRating = ratings.UserRating;
         return movie;
    }

    public Task<bool> DeleteAsync(Guid id , CancellationToken cancellationToken =default)
    {
        return _movieRepository.DeleteAsync(id , cancellationToken);
    }
}