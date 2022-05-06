using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Services.Interfaces;
using BugTracksV3.Services;
using BugTracker.Services;
using BugTracksV3.Utilities;
using Microsoft.Extensions.DependencyInjection;
using BugTracker.Services.Factories;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(DataUtility.GetConnectionString(builder.Configuration), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<BTUserClaimsPrincipalFactory>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Custom Services
builder.Services.AddScoped<IBTRolesService, BTRolesService>();
//services.AddScoped<IBTCompanyInfoService, BTCompanyInfoService>();
//services.AddScoped<IBTProjectService, BTProjectService>();
//services.AddScoped<IBTTicketService, BTTicketService>();
//services.AddScoped<IBTTicketHistoryService, BTTicketHistoryService>();
//services.AddScoped<IBTNotificationService, BTNotificationService>();
//services.AddScoped<IBTInviteService, BTInviteService>();
//services.AddScoped<IBTFileService, BTFileService>();
//services.AddScoped<IBTLookupService, BTLookupService>();

var app = builder.Build();

await DataUtility.ManageDataAsync(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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