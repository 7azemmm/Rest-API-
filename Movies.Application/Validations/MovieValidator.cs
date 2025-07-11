using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validations;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;
    
    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(M => M.id).NotEmpty();
        RuleFor(m => m.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(m => m.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
        RuleFor(M=>M.slug).MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system");
    

    }

    private async Task<bool> ValidateSlug(Movie movie , string slug , CancellationToken cancellationToken =default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug);

        if (existingMovie is not null)
        {
            return existingMovie.id == movie.id;
        }

        return existingMovie is null;
    }
}


