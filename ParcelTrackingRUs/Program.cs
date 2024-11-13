using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ParcelTrackingRUs.Models;
using ParcelTrackingRUs.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<LocationService>();

// Add Swagger for API documentation (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define API endpoints
var locations = app.MapGroup("/api/track");

locations.MapPost("/{entityId}/location", async (string entityId, Location location, LocationService locationService) =>
{
    location.EntityId = entityId;
    location.Timestamp = DateTime.Now; // set the current timestamp

    await locationService.CreateAsync(location);
    return Results.Created($"/api/track/{entityId}/location", location);
})
.WithName("CreateLocation");

locations.MapGet("/{entityId}/location/latest", async (string entityId, LocationService locationService) =>
{
    var location = await locationService.GetLatestLocationAsync(entityId);

    return location is not null
        ? Results.Ok(location)
        : Results.NotFound();
})
.WithName("GetLatestLocation");

app.Run();
