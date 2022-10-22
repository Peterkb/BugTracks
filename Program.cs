using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracksV3.Utilities;
using Microsoft.Extensions.DependencyInjection;
using BugTracker.Services.Factories;
using BugTrackerNet5Mvc.Services.Interfaces;
using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Services.Interfaces;
using BugTracksV3.Services;
using BugTracker.Services;
using BugTracksV3.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using BugTracksV3.Configurations;

var builder = WebApplication.CreateBuilder(args);

//Abstract Startup Configuration
builder.AddStandardServices();

builder.AddDatabaseServices();
builder.AddAuthServices();

builder.AddCustomServices();

builder.AddEmailServices();

var app = builder.Build();

//DBs
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
await DataUtility.ManageDataAsync(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
