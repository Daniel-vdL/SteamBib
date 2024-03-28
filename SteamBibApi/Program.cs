using Microsoft.EntityFrameworkCore;
using SteamBibApi.Models;

var builder = WebApplication.CreateBuilder(args);

using var db = new AppDbContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var steamApiHandler = new SteamApiHandler();
var appList = await steamApiHandler.GetAppsAsync();

db.Apps.ExecuteDelete();

app.Run();