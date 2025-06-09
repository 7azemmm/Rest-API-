using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(x => x.id == id);
        return Task.FromResult(movie);
    }

    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var movieIndex = _movies.FindIndex(x=> x.id == movie.id);
        if (movieIndex == -1)
        {
            return Task.FromResult(false);
        }
        _movies[movieIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
      var removedCount = _movies.RemoveAll(x => x.id == id);
      var movieDeleted = removedCount > 0;
      return Task.FromResult(movieDeleted);
    }
}