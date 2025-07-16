using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace RestApi.Controllers;

[ApiController]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut(ApiEndpoints.Movies.rate)]
    public async Task<IActionResult> RateMovie( [FromRoute] Guid MovieId,
    [FromBody] RateMovieRequest ratingRequest , CancellationToken token =default)
    {
        var UserId = HttpContext.GetUserId();
        Console.WriteLine($"UserId: {UserId}");
        Console.WriteLine($"MovieId: {MovieId}");
        var result = await _ratingService.RateMovieAsync(MovieId , ratingRequest.rating , UserId!.Value , token);
        return result ? Ok() : NotFound();
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.deleteRating)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid MovieId)
    {
        var UserId = HttpContext.GetUserId();
        var result = await _ratingService.DeleteMovieAsync(MovieId , UserId);
        return result ? Ok("ok deleted") : NotFound();
    }
    
}