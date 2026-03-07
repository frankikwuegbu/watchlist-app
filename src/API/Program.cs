using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Watchlist App";
        options.HideClientButton = true;
    });
}

app.UseHttpsRedirection();

app.MapGet("/", () => "hello world");

app.Run();