using Watchlist_App.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.MapGet("/", () => "hello world");

app.Run();