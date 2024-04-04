using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SteamBibApi.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

using var db = new AppDbContext();
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

//db.SteamApps.ExecuteDelete();

//await FillSteamApps();

app.Run();

//async Task FillSteamApps()
//{
//    var steamApiHandler = new SteamApiHandler();
//    var appList = await steamApiHandler.GetAppsAsync();

//    foreach (var apps in appList.Applist.Apps)
//    {
//        var name = apps.Name;
//        var appId = apps.Appid;
//        var steamApp = new SteamApp
//        {
//            Appid = apps.Appid,
//            Name = apps.Name
//        };

//        db.SteamApps.Add(steamApp);
//    }

//    await db.SaveChangesAsync();

//    Console.WriteLine("Steam apps data filled successfully.");
//}