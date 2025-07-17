using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validations;

public class GetAllMoviesValidator : AbstractValidator<GetAllMoviesOptions>
{
   public GetAllMoviesValidator()
   {
      RuleFor( x => x.YearOfRealease).LessThanOrEqualTo(DateTime.UtcNow.Year);
   }
}