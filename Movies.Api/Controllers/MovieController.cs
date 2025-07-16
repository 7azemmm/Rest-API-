using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using RestApi.Mapping;

namespace RestApi.Controllers;

[ApiController]

public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieRepository)
    {
        _movieService = movieRepository;
    }

    [HttpPost(ApiEndpoints.Movies.create)]
    // [Authorize(AuthConstants.AdminUserPolicyName)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request , CancellationToken token)
    {
        var movie = request.MapToMovie();
        await _movieService.CreateAsync(movie , token);
        return CreatedAtAction(nameof(Get), new { idOrslug = movie.id }, movie);
        // return Created($"{ApiEndpoints.Movies.create}/{movie.id}", movie);
    }
    [HttpGet(ApiEndpoints.Movies.get)]
    public async Task<IActionResult> Get([FromRoute] string idOrslug , CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = Guid.TryParse(idOrslug , out var id ) ? 
                 await _movieService.GetByIdAsync(id ,token , userId) : 
                 await _movieService.GetBySlugAsync(idOrslug ,token, userId);
        if (movie == null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.getall)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movies = await _movieService.GetAllAsync(token , userId);
        var response = movies.MapToResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.Movies.update)]
    public async Task<IActionResult> Update([FromBody] UpdateMovieRequest request ,[FromRoute] Guid id , CancellationToken token )
    {
        var UserId = HttpContext.GetUserId();
        var movie = request.MapToMovie(id);
        var checkUpdateMovie = await _movieService.UpdateAsync(movie , UserId , token );
        if (checkUpdateMovie is null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);

    }

    [HttpDelete(ApiEndpoints.Movies.delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var DeletedMovie = await _movieService.DeleteAsync(id);
        return DeletedMovie ? Ok() : NotFound();
    }
}