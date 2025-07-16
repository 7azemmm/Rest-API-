using Movies.Application.Models;

namespace Movies.Application.Repositories;
using Movies.Application.Database;
using Dapper;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMovieRepository _movieRepository;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory , IMovieRepository movieRepository)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<float?> getRatingAsync(Guid movieId, CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            select round(avg(r.rating), 1) from ratings r
            where movieid = @movieId
            """, new { movieId }, cancellationToken: cancellationToken));

    }
    

    public async Task<(float? Rating, int? UserRating)> getRatingAsync(Guid movieId, Guid userId, CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            select round(avg(rating), 1), 
                   (select rating 
                    from ratings 
                    where movieid = @movieId 
                      and userid = @userId
                    limit 1) 
            from ratings
            where movieid = @movieId
            """, new { movieId, userId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid? userId, CancellationToken token = default)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         insert into ratings(userid, movieid, rating) 
                                                                         values (@userId, @movieId, @rating)
                                                                         on conflict (userid, movieid) do update 
                                                                             set rating = @rating
                                                                         """, new { userId, movieId, rating }, cancellationToken: token));

        return result > 0;
        
    }

    public async Task<bool> DeleteMovieAsync(Guid movieId, Guid? userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.ExecuteAsync(new CommandDefinition
            (
            """
            Delete from ratings where movieid = @movieId and userid = @userId
            """ , new{movieId , userId} , cancellationToken: token
            )
        );
        return result > 0;

    }

    public async Task<IEnumerable<MovieRatings>> getUserRatingsAsync(Guid? userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QueryAsync<MovieRatings>(new CommandDefinition(
            """
             select r.rating , r.movieid , m.slug from ratings r
             inner join movies m on m.id = r.movieid
             where userid = @userId;

            """, new { userId }, cancellationToken: token));
    }

}