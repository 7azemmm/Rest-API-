namespace Movies.Application.Services;
using Movies.Application.Models;
public interface IMovieService
{
    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken =default , Guid? userId = default );
    public Task<Movie?> GetByIdAsync(Guid id , CancellationToken cancellationToken =default , Guid? userId = default);
    
    public Task<Movie?> GetBySlugAsync(string slug , CancellationToken cancellationToken =default , Guid? userId = default);
    public Task<bool> CreateAsync(Movie movie , CancellationToken cancellationToken =default);
    public Task<Movie?> UpdateAsync(Movie movie , CancellationToken cancellationToken =default , Guid? userId = default);
    public Task<bool> DeleteAsync(Guid id , CancellationToken cancellationToken =default);
}