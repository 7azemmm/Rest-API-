using System.Runtime.InteropServices;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using RestApi.Mapping;

namespace RestApi.Controllers.v2;

[ApiController]

public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [ApiVersion(2.0)]
    [HttpGet(ApiEndpoints.v2.Movies.get)]
    public async Task<IActionResult> Get([FromRoute] string idOrslug , CancellationToken token)
    {
        Console.WriteLine("hello from v2");
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

    
   
}