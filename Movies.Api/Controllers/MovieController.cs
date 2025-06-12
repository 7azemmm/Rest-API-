using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using RestApi.Mapping;

namespace RestApi.Controllers;

[ApiController]

public class MovieController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MovieController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost(ApiEndpoints.Movies.create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();

        await _movieRepository.CreateAsync(movie);
        return CreatedAtAction(nameof(Get), new { id = movie.id }, movie);
        // return Created($"{ApiEndpoints.Movies.create}/{movie.id}", movie);
    }
    [HttpGet(ApiEndpoints.Movies.get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.getall)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();
        var response = movies.MapToResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.Movies.update)]
    public async Task<IActionResult> Update([FromBody] UpdateMovieRequest request ,[FromRoute] Guid id)
    {
        var movie = request.MapToMovie(id);
        var checkUpdateMovie = await _movieRepository.UpdateAsync(movie);
        if (!checkUpdateMovie)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);

    }

    [HttpDelete(ApiEndpoints.Movies.delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var DeletedMovie = await _movieRepository.DeleteAsync(id);
        return DeletedMovie ? Ok() : NotFound();
    }
}