using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validations;

public class GetAllMoviesValidator : AbstractValidator<GetAllMoviesOptions>
{
   private static readonly string[] AcceptableSortFields = { "title", "yearofrelease" };
   public GetAllMoviesValidator()
   {
      RuleFor( x => x.YearOfRealease).LessThanOrEqualTo(DateTime.UtcNow.Year);
      RuleFor(x => x.sortField)
         .Must(x => x is null || AcceptableSortFields.Contains(x , StringComparer.OrdinalIgnoreCase))
         .WithMessage("sort field is only title or yearofrelease");
   }
}