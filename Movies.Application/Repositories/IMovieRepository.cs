using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken =default);
    public Task<Movie?> GetByIdAsync(Guid id , CancellationToken cancellationToken =default);
    
    public Task<Movie?> GetBySlugAsync(string slug ,CancellationToken cancellationToken =default);
    public Task<bool> CreateAsync(Movie movie , CancellationToken cancellationToken =default);
    public Task<bool> UpdateAsync(Movie movie , CancellationToken cancellationToken =default);
    public Task<bool> DeleteAsync(Guid id , CancellationToken cancellationToken =default);
    public Task<bool> ExistbyIdAsync(Guid id ,CancellationToken cancellationToken =default);
}