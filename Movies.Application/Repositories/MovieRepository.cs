
using Movies.Application.Database;
using Movies.Application.Models;
using Dapper;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MovieRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory; 
        
    }
    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token =default , GetAllMoviesOptions? options = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
                                                                       select m.*, 
                                                                              string_agg(distinct g.name, ',') as genres , 
                                                                              round(avg(r.rating), 1) as rating, 
                                                                              myr.rating as userrating
                                                                       from movies m 
                                                                       left join genres g on m.id = g.movieid
                                                                       left join ratings r on m.id = r.movieid
                                                                       left join ratings myr on m.id = myr.movieid
                                                                           and myr.userid = @userId
                                                                       where (@title is null or m.title like ('%' || @title || '%'))
                                                                       and (@yearofRelease is null or m.yearofrelease = @yearofRelease)
                                                                       group by id, userrating
                                                                       """, new
        {
            userId = options.UserId,
            title = options.title,
            yearofrelease = options.YearOfRealease
        }, cancellationToken: token));
        
        return result.Select(x => new Movie
        {
            id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Rating = (float?)x.rating,
            userRating = (int?)x.userrating,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
        
    }

    public async Task<Movie?> GetByIdAsync(Guid id , CancellationToken token=default , Guid? userId= default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  select m.*, round(avg(r.rating), 1) as rating, myr.rating as userrating 
                                  from movies m
                                  left join ratings r on m.id = r.movieid
                                  left join ratings myr on m.id = myr.movieid
                                      and myr.userid = @userId
                                  where id = @id
                                  group by id, userrating
                                  """, new { id, userId }, cancellationToken: token));

        if (movie is null)
        {
            return null;
        }
        
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name from genres where movieid = @id 
                                  """, new { id }, cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }
    
    public async Task<Movie?> GetBySlugAsync(string Slug , CancellationToken token =default , Guid? userId= default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  select m.*, round(avg(r.rating), 1) as rating, myr.rating as userrating
                                  from movies m
                                  left join ratings r on m.id = r.movieid
                                  left join ratings myr on m.id = myr.movieid
                                      and myr.userid = @userId
                                  where slug = @slug
                                  group by id, userrating
                                  """, new { Slug, userId }, cancellationToken: token));

        if (movie is null)
        {
            return null;
        }
        
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name from genres where movieid = @id 
                                  """, new { id = movie.id }, cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
       
    }

    public async Task<bool> CreateAsync(Movie movie , CancellationToken token =default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         insert into movies (id, slug, title, yearofrelease) 
                                                                         values (@Id, @Slug, @Title, @YearOfRelease)
                                                                         """, movie , cancellationToken:token));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                                                                    insert into genres (movieId, name) 
                                                                    values (@MovieId, @Name)
                                                                    """, new { MovieId = movie.id, Name = genre }));
            }
        }
        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> UpdateAsync(Movie movie ,CancellationToken token =default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = movie.id } , cancellationToken:token));
        
        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                                                                insert into genres (movieId, name) 
                                                                values (@MovieId, @Name)
                                                                """, new { MovieId = movie.id, Name = genre } , cancellationToken:token));
        }
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
                                                                         where id = @Id
                                                                         """, movie , cancellationToken:token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id , CancellationToken token =default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id } , cancellationToken:token));
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         delete from movies where id = @id
                                                                         """, new { id } ,cancellationToken:token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistbyIdAsync(Guid id , CancellationToken token =default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from movies where id = @id
                                                                               """, new { id } , cancellationToken:token));
    }
}