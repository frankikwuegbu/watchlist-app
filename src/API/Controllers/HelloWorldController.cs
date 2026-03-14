using Microsoft.AspNetCore.Mvc;

namespace Watchlist_App.Controllers;

[ApiController]
public class HelloWorldController : ControllerBase
{
    [HttpGet("helloWorld")]
    public string HelloWorld()
    {
        return "Hello world";
    }
}
