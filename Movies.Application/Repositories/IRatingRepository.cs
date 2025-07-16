namespace Movies.Application.Repositories;

public interface IRatingRepository
{
    public Task<float?> getRatingAsync(Guid movieId, CancellationToken cancellationToken);
    public Task<(float? Rating, int? UserRating)> getRatingAsync(Guid movieId, Guid userId ,  CancellationToken cancellationToken);
    
    public Task<bool> RateMovieAsync(Guid movieId, int rating, Guid? userId, CancellationToken token = default);
}