using Application.Common;
using Application.Watchlists.Command;
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
}
