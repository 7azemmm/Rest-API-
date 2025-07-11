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
    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return _movieRepository.GetAllAsync();
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        return _movieRepository.GetByIdAsync(id);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        return _movieRepository.GetBySlugAsync(slug);
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
         await _movievalidator.ValidateAndThrowAsync(movie);
        return await _movieRepository.CreateAsync(movie);
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        await _movievalidator.ValidateAndThrowAsync(movie);
        var MovieExist = await _movieRepository.ExistbyIdAsync(movie.id);
        if (!MovieExist)
        {
            return null;
        }
         await _movieRepository.UpdateAsync(movie);
         return movie;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return _movieRepository.DeleteAsync(id);
    }
}