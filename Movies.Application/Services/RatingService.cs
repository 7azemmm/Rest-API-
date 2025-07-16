using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Repositories;
using Movies.Application.Models;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _Ratingrepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository repository , IMovieRepository movieRepository)
    {
        _Ratingrepository = repository;
        _movieRepository = movieRepository;
    }
    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid? userId, CancellationToken token = default)
    {
        if (rating is <=0 or > 5)
        {
            throw new ValidationException(new[]
                {
                   new ValidationFailure()
                   {
                       PropertyName = "Rating",
                       ErrorMessage = "please Insert rating in range between 0 and 5",
                   }
                }
            );
        }
        
        var MovieExist = await _movieRepository.ExistbyIdAsync(movieId, token);
        if (!MovieExist)
        {
            Console.WriteLine("Movie not found");
            return false;
        }
        return await _Ratingrepository.RateMovieAsync(movieId, rating, userId, token);
    }

    public async Task<bool> DeleteMovieAsync(Guid movieId, Guid? UserId, CancellationToken token = default)
    {
        return await _Ratingrepository.DeleteMovieAsync(movieId, UserId, token);
    }

    public Task<IEnumerable<MovieRatings>> getUserRatingsAsync(Guid? userId, CancellationToken token = default)
    {
        return  _Ratingrepository.getUserRatingsAsync(userId);
    }

}