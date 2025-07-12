using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Validations;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movievalidator;
    public MovieService(IMovieRepository movieRepository , IValidator<Movie> validator)
    {
        _movieRepository = movieRepository;
        _movievalidator = validator;
    }
    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken =default)
    {
        return _movieRepository.GetAllAsync(cancellationToken);
    }

    public Task<Movie?> GetByIdAsync(Guid id , CancellationToken cancellationToken =default)
    {
        return _movieRepository.GetByIdAsync(id , cancellationToken);
    }

    public Task<Movie?> GetBySlugAsync(string slug , CancellationToken cancellationToken =default)
    {
        return _movieRepository.GetBySlugAsync(slug , cancellationToken);
    }

    public async Task<bool> CreateAsync(Movie movie , CancellationToken cancellationToken =default)
    {
         await _movievalidator.ValidateAndThrowAsync(movie);
        return await _movieRepository.CreateAsync(movie , cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie , CancellationToken cancellationToken =default)
    {
        await _movievalidator.ValidateAndThrowAsync(movie);
        var MovieExist = await _movieRepository.ExistbyIdAsync(movie.id , cancellationToken);
        if (!MovieExist)
        {
            return null;
        }
         await _movieRepository.UpdateAsync(movie);
         return movie;
    }

    public Task<bool> DeleteAsync(Guid id , CancellationToken cancellationToken =default)
    {
        return _movieRepository.DeleteAsync(id , cancellationToken);
    }
}