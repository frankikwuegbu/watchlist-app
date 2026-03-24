using Application.Common;
using Application.Movies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Watchlist_App.Controllers;

[ApiController]
[Route("movies")]
public class MoviesController : ControllerBase
{
    private readonly ISender _sender;

    public MoviesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("title")]
    public async Task<ActionResult<Result>> GetByTitle(string title)
    {
        return await _sender.Send(new GetByTitleQuery(title));
    }

    [HttpGet("details")]
    public async Task<ActionResult<Result>> GetDetailsById(int id, string mediaType)
    {
        return await _sender.Send(new GetDetailsByIdQuery(id, mediaType));
    }
}