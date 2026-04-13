using Application.Common;
using Application.Watchlists.Command;
using Application.Watchlists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Watchlist_App.Controllers;

[ApiController]
[Route("watchlist")]
public class WatchlistController
{
    private readonly ISender _sender;

    public WatchlistController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("addtowatchlist")]
    public async Task<ActionResult<Result>> AddToWatchlist(AddToWatchlistCommand command)
    {
        return await _sender.Send(command);
    }

    [HttpGet("getallmoviesinwatchlist")]
    public async Task<ActionResult<Result>> GetWatchlistMovies()
    {
        return await _sender.Send(new GetAllMoviesInWatchlistQuery());
    }

    [HttpDelete("removefromwatchlist")]
    public async Task<ActionResult<Result>> RemoveFromWatchlist(int id)
    {
        return await _sender.Send(new DeleteFromWatchlistQuery(Id: id));
    }
}
