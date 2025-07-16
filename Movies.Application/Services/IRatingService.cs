namespace Movies.Application.Services;

public interface IRatingService
{
    public Task<bool> RateMovieAsync(Guid movieId, int rating, Guid? userId, CancellationToken token = default);
}