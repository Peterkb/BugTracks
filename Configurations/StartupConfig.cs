using BugTracker.Services;
using BugTrackerNet5Mvc.Services.Interfaces;
using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Services.Interfaces;
using BugTracksV3.Services;
using BugTracksV3.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using BugTracker.Services.Factories;
using Microsoft.AspNetCore.Identity;
using BugTracksV3.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BugTracksV3.Configurations;

public static class StartupConfig
{
	public static void AddStandardServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllersWithViews();
	}

	public static void AddDatabaseServices(this WebApplicationBuilder builder)
	{
		var connectionString = DataUtility.GetConnectionString(builder.Configuration);

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
	}

	public static void AddCustomServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IBTRolesService, BTRolesService>();
		builder.Services.AddScoped<IBTCompanyInfoService, BTCompanyInfoService>();
		builder.Services.AddScoped<IBTProjectService, BTProjectService>();
		builder.Services.AddScoped<IBTTicketService, BTTicketService>();
		builder.Services.AddScoped<IBTTicketHistoryService, BTTicketHistoryService>();
		builder.Services.AddScoped<IBTNotificationService, BTNotificationService>();
		builder.Services.AddScoped<IBTInviteService, BTInviteService>();
		builder.Services.AddScoped<IBTFileService, BTFileService>();
		builder.Services.AddScoped<IBTLookupService, BTLookupService>();
	}
	public static void AddEmailServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IEmailSender, BTEmailService>();
		builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
	}

	public static void AddAuthServices(this WebApplicationBuilder builder)
	{
		var configuration = builder.Configuration;

		// Google
		builder.Services.AddAuthentication()
			.AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = configuration["Google:ClientId"];
				googleOptions.ClientSecret = configuration["Google:ClientSecret"];
			});

		builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddClaimsPrincipalFactory<BTUserClaimsPrincipalFactory>()
			.AddDefaultUI()
			.AddDefaultTokenProviders();
	}
}
