namespace Movies.Application.Services;
using Movies.Application.Models;
public interface IMovieService
{
    public Task<IEnumerable<Movie>> GetAllAsync();
    public Task<Movie?> GetByIdAsync(Guid id);
    
    public Task<Movie?> GetBySlugAsync(string slug);
    public Task<bool> CreateAsync(Movie movie);
    public Task<Movie?> UpdateAsync(Movie movie);
    public Task<bool> DeleteAsync(Guid id);
}