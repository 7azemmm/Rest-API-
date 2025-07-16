using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IRatingService
{
    public Task<bool> RateMovieAsync(Guid movieId, int rating, Guid? userId, CancellationToken token = default);
    
    public Task<bool> DeleteMovieAsync(Guid movieId, Guid? userId, CancellationToken token = default);
    
    public Task<IEnumerable<MovieRatings>> getUserRatingsAsync(Guid? userId, CancellationToken token = default);
}