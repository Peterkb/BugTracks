﻿using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Models;
using BugTracksV3.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BugTracksV3.Utilities;
public class DataUtility
{
	//Connection String Builder
	public static string GetConnectionString(IConfiguration configuration)
	{
		var connectionString = configuration.GetSection("pgSettings")["pgConnection"];
		var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

		return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
	}

	private static string BuildConnectionString(string databaseUrl)
	{
		var databaseUri = new Uri(databaseUrl);
		var userInfo = databaseUri.UserInfo.Split(':');
		var builder = new NpgsqlConnectionStringBuilder
		{
			Host = databaseUri.Host,
			Port = databaseUri.Port,
			Username = userInfo[0],
			Password = userInfo[1],
			Database = databaseUri.LocalPath.TrimStart('/'),
			SslMode = SslMode.Require,
			TrustServerCertificate = true
		};
		return builder.ToString();
	}

	//Seeding Database
	private static int company1Id;

	public static async Task ManageDataAsync(IHost host)
	{
		var svcScope = host.Services.CreateScope();
		var svcProvider = svcScope.ServiceProvider;
		//Service: An instance of RoleManager
		var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

		//Service: An instance of RoleManager
		var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

		//Service: An instance of the UserManager
		var userManagerSvc = svcProvider.GetRequiredService<UserManager<ApplicationUser>>();

		//Migration: This is the programmatic equivalent to Update-Database
		await dbContextSvc.Database.MigrateAsync();

		await SeedRolesAsync(roleManagerSvc);
		await SeedDefaultCompaniesAsync(dbContextSvc);
		await SeedDefaultUsersAsync(userManagerSvc);
		await SeedDemoUsersAsync(userManagerSvc);
		await SeedDefaultTicketTypeAsync(dbContextSvc);
		await SeedDefaultTicketStatusAsync(dbContextSvc);
		await SeedDefaultTicketPriorityAsync(dbContextSvc);
		await SeedDefaultProjectPriorityAsync(dbContextSvc);
		await SeedDefautProjectsAsync(dbContextSvc);
		await SeedDefautTicketsAsync(dbContextSvc);
	}

	//Users
	#region Seed Roles
	public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
	{
		//Seed Roles
		await roleManager.CreateAsync(new IdentityRole(Roles.SuperUser.ToString()));
		await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
		await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
		await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
		await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
		await roleManager.CreateAsync(new IdentityRole(Roles.DemoUser.ToString()));
	}
	#endregion

	#region Seed Companies
	public static async Task SeedDefaultCompaniesAsync(ApplicationDbContext context)
	{
		try
		{
			IList<Company> defaultcompanies = new List<Company>() {
				new Company() { Name = "Brede Code", Description="Personal Coding Space" },
			};

			var dbCompanies = context.Companies.Select(c => c.Name).ToList();
			await context.Companies.AddRangeAsync(defaultcompanies.Where(c => !dbCompanies.Contains(c.Name)));
			await context.SaveChangesAsync();

			//Get company Ids
			company1Id = context.Companies.FirstOrDefault(p => p.Name == "Brede Code").Id;
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Companies.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	#region Seed Default Users
	public static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager)
	{
		//Seed Default Admin User
		var defaultUser = new ApplicationUser
		{
			UserName = "btadmin@bugtracker.com",
			Email = "btadmin@bugtracker.com",
			FirstName = "Peter",
			LastName = "Bredell",
			EmailConfirmed = true,
			CompanyId = company1Id
		};
		try
		{
			var user = await userManager.FindByEmailAsync(defaultUser.Email);
			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "Abc&123!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Default Admin User.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}

		//Seed Default ProjectManager User
		defaultUser = new ApplicationUser
		{
			UserName = "ProjectManager1@bugtracker.com",
			Email = "ProjectManager1@bugtracker.com",
			FirstName = "John",
			LastName = "Appuser",
			EmailConfirmed = true,
			CompanyId = company1Id
		};
		try
		{
			var user = await userManager.FindByEmailAsync(defaultUser.Email);
			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "Abc&123!");
				await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Default ProjectManager1 User.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}

		// Seed Default Developer1 User
		defaultUser = new ApplicationUser
		{
			UserName = "Developer1@bugtracker.com",
			Email = "Developer1@bugtracker.com",
			FirstName = "Elon",
			LastName = "Appuser",
			EmailConfirmed = true,
			CompanyId = company1Id
		};
		try
		{
			var user = await userManager.FindByEmailAsync(defaultUser.Email);
			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "Abc&123!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Default Developer1 User.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}

		//Seed Default Submitter1 User
		defaultUser = new ApplicationUser
		{
			UserName = "Submitter1@bugtracker.com",
			Email = "Submitter1@bugtracker.com",
			FirstName = "Scott",
			LastName = "Appuser",
			EmailConfirmed = true,
			CompanyId = company1Id
		};
		try
		{
			var user = await userManager.FindByEmailAsync(defaultUser.Email);
			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "Abc&123!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Default Submitter1 User.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	#region Seed Demo Users
	public static async Task SeedDemoUsersAsync(UserManager<ApplicationUser> userManager)
	{
		//Seed Demo Admin User
		var defaultUser = new ApplicationUser
		{
			UserName = "demoadmin@bugtracker.com",
			Email = "demoadmin@bugtracker.com",
			FirstName = "Demo",
			LastName = "Admin",
			EmailConfirmed = true,
			CompanyId = company1Id
		};
		try
		{
			var user = await userManager.FindByEmailAsync(defaultUser.Email);
			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "Abc&123!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
				await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Demo Admin User.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	//Projects
	#region Seed Default Projects
	public static async Task SeedDefaultProjectPriorityAsync(ApplicationDbContext context)
	{
		try
		{
			IList<ProjectPriority> projectPriorities = new List<ProjectPriority>() {
												new ProjectPriority() { Name = BTProjectPriority.Low.ToString() },
												new ProjectPriority() { Name = BTProjectPriority.Medium.ToString() },
												new ProjectPriority() { Name = BTProjectPriority.High.ToString() },
												new ProjectPriority() { Name = BTProjectPriority.Urgent.ToString() },
			};

			var dbProjectPriorities = context.ProjectPriorities.Select(c => c.Name).ToList();
			await context.ProjectPriorities.AddRangeAsync(projectPriorities.Where(c => !dbProjectPriorities.Contains(c.Name)));
			await context.SaveChangesAsync();

		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Project Priorities.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}

	public static async Task SeedDefautProjectsAsync(ApplicationDbContext context)
	{

		//Get project priority Ids
		int priorityLow = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Low.ToString()).Id;
		int priorityMedium = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Medium.ToString()).Id;
		int priorityHigh = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.High.ToString()).Id;
		int priorityUrgent = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Urgent.ToString()).Id;

		try
		{
			IList<Project> projects = new List<Project>() {
				 new Project()
				 {
					 CompanyId = company1Id,
					 Name = "Personal Porfolio",
					 Description="React Website using React Three fiber. Serves as a landing page for my personal portfolio." ,
					 StartDate = new DateTime(2021,8,20),
					 EndDate = new DateTime(2021,8,20).AddMonths(1),
					 ProjectPriorityId = priorityLow
				 },
				 new Project()
				 {
					 CompanyId = company1Id,
					 Name = "Blog Drops",
					 Description="My custom built web application using .Net Core with MVC, a postgres database and hosted in a railway container.  Made to create, update and maintain a live blog site.",
					 StartDate = new DateTime(2021,8,20),
					 EndDate = new DateTime(2021,8,20).AddMonths(4),
					 ProjectPriorityId = priorityMedium
				 },
				 new Project()
				 {
					 CompanyId = company1Id,
					 Name = "TicketFlux",
					 Description="A custom designed .Net Core application with postgres database. The application is a Multi-tenant application designed to track issue tickets' progress.  Implemented with identity and user roles, Tickets are maintained in projects which are maintained by users in the role of projectmanager.  Each project has a team and team members.",
					 StartDate = new DateTime(2021,8,20),
					 EndDate = new DateTime(2021,8,20).AddMonths(6),
					 ProjectPriorityId = priorityHigh
				 },
				 new Project()
				 {
					 CompanyId = company1Id,
					 Name = "Contact Crate",
					 Description="A custom designed .Net Core application with postgres database.  This is an application to serve as a rolodex of contacts for a given user.",
					 StartDate = new DateTime(2021,8,20),
					 EndDate = new DateTime(2021,8,20).AddMonths(2),
					 ProjectPriorityId = priorityMedium
				 },
				new Project()
				 {
					 CompanyId = company1Id,
					 Name = "Movie Pro",
					 Description="A custom designed .Net Core application with postgres database.  An API based application allows users to input and import movie posters and details including cast and crew information.",
					 StartDate = new DateTime(2021,8,20),
					 EndDate = new DateTime(2021,8,20).AddMonths(3),
					 ProjectPriorityId = priorityLow
				 }
			};

			var dbProjects = context.Projects.Select(c => c.Name).ToList();
			await context.Projects.AddRangeAsync(projects.Where(c => !dbProjects.Contains(c.Name)));
			await context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Projects.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	} 
	#endregion

	//Tickets
	#region Seed Default Ticket Types
	public static async Task SeedDefaultTicketTypeAsync(ApplicationDbContext context)
	{
		try
		{
			IList<TicketType> ticketTypes = new List<TicketType>() {
				 new TicketType() { Name = BTTicketType.NewDevelopment.ToString() },      // Ticket involves development of a new, uncoded solution 
				 new TicketType() { Name = BTTicketType.WorkTask.ToString() },            // Ticket involves development of the specific ticket description 
				 new TicketType() { Name = BTTicketType.Defect.ToString()},               // Ticket involves unexpected development/maintenance on a previously designed feature/functionality
				 new TicketType() { Name = BTTicketType.ChangeRequest.ToString() },       // Ticket involves modification development of a previously designed feature/functionality
				 new TicketType() { Name = BTTicketType.Enhancement.ToString() },         // Ticket involves additional development on a previously designed feature or new functionality
				 new TicketType() { Name = BTTicketType.GeneralTask.ToString() }          // Ticket involves no software development but may involve tasks such as configurations, or hardware setup
			};

			var dbTicketTypes = context.TicketTypes.Select(c => c.Name).ToList();
			await context.TicketTypes.AddRangeAsync(ticketTypes.Where(c => !dbTicketTypes.Contains(c.Name)));
			await context.SaveChangesAsync();

		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Ticket Types.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	#region Seed Default Ticket Status
	public static async Task SeedDefaultTicketStatusAsync(ApplicationDbContext context)
	{
		try
		{
			IList<TicketStatus> ticketStatuses = new List<TicketStatus>() {
				new TicketStatus() { Name = BTTicketStatus.New.ToString() },                 // Newly Created ticket having never been assigned
				new TicketStatus() { Name = BTTicketStatus.Development.ToString() },         // Ticket is assigned and currently being worked 
				new TicketStatus() { Name = BTTicketStatus.Testing.ToString()  },            // Ticket is assigned and is currently being tested
				new TicketStatus() { Name = BTTicketStatus.Resolved.ToString()  },           // Ticket remains assigned to the developer but work is now complete
			};

			var dbTicketStatuses = context.TicketStatuses.Select(c => c.Name).ToList();
			await context.TicketStatuses.AddRangeAsync(ticketStatuses.Where(c => !dbTicketStatuses.Contains(c.Name)));
			await context.SaveChangesAsync();

		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Ticket Statuses.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	#region Seed Default Ticket Priority
	public static async Task SeedDefaultTicketPriorityAsync(ApplicationDbContext context)
	{
		try
		{
			IList<TicketPriority> ticketPriorities = new List<TicketPriority>() {
												new TicketPriority() { Name = BTTicketPriority.Low.ToString()  },
												new TicketPriority() { Name = BTTicketPriority.Medium.ToString() },
												new TicketPriority() { Name = BTTicketPriority.High.ToString()},
												new TicketPriority() { Name = BTTicketPriority.Urgent.ToString()},
			};

			var dbTicketPriorities = context.TicketPriorities.Select(c => c.Name).ToList();
			await context.TicketPriorities.AddRangeAsync(ticketPriorities.Where(c => !dbTicketPriorities.Contains(c.Name)));
			context.SaveChanges();

		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Ticket Priorities.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion

	#region Seed Default Tickets
	public static async Task SeedDefautTicketsAsync(ApplicationDbContext context)
	{
		//Get project Ids
		int portfolioId = context.Projects.FirstOrDefault(p => p.Name == "Build a Personal Porfolio").Id;
		int blogId = context.Projects.FirstOrDefault(p => p.Name == "Build a supplemental Blog Web Application").Id;
		int bugtrackerId = context.Projects.FirstOrDefault(p => p.Name == "Build an Issue Tracking Web Application").Id;
		int movieId = context.Projects.FirstOrDefault(p => p.Name == "Build a Movie Information Web Application").Id;

		//Get ticket type Ids
		int typeNewDev = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.NewDevelopment.ToString()).Id;
		int typeWorkTask = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.WorkTask.ToString()).Id;
		int typeDefect = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.Defect.ToString()).Id;
		int typeEnhancement = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.Enhancement.ToString()).Id;
		int typeChangeRequest = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.ChangeRequest.ToString()).Id;

		//Get ticket priority Ids
		int priorityLow = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Low.ToString()).Id;
		int priorityMedium = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Medium.ToString()).Id;
		int priorityHigh = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.High.ToString()).Id;
		int priorityUrgent = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Urgent.ToString()).Id;

		//Get ticket status Ids
		int statusNew = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.New.ToString()).Id;
		int statusDev = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Development.ToString()).Id;
		int statusTest = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Testing.ToString()).Id;
		int statusResolved = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Resolved.ToString()).Id;

		try
		{
			IList<Ticket> tickets = new List<Ticket>() {
				//PORTFOLIO
				new Ticket() {Title = "Portfolio Ticket 1", Description = "Ticket details for portfolio ticket 1", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Portfolio Ticket 2", Description = "Ticket details for portfolio ticket 2", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Portfolio Ticket 3", Description = "Ticket details for portfolio ticket 3", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Portfolio Ticket 4", Description = "Ticket details for portfolio ticket 4", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityUrgent, TicketStatusId = statusTest, TicketTypeId = typeDefect},
				new Ticket() {Title = "Portfolio Ticket 5", Description = "Ticket details for portfolio ticket 5", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Portfolio Ticket 6", Description = "Ticket details for portfolio ticket 6", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Portfolio Ticket 7", Description = "Ticket details for portfolio ticket 7", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Portfolio Ticket 8", Description = "Ticket details for portfolio ticket 8", DateCreated = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = priorityUrgent, TicketStatusId = statusTest, TicketTypeId = typeDefect},
				//BLOG
				new Ticket() {Title = "Blog Ticket 1", Description = "Ticket details for blog ticket 1", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeDefect},
				new Ticket() {Title = "Blog Ticket 2", Description = "Ticket details for blog ticket 2", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityMedium, TicketStatusId = statusDev, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Blog Ticket 3", Description = "Ticket details for blog ticket 3", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Blog Ticket 4", Description = "Ticket details for blog ticket 4", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Blog Ticket 5", Description = "Ticket details for blog ticket 5", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityLow, TicketStatusId = statusDev,  TicketTypeId = typeDefect},
				new Ticket() {Title = "Blog Ticket 6", Description = "Ticket details for blog ticket 6", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew,  TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Blog Ticket 7", Description = "Ticket details for blog ticket 7", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Blog Ticket 8", Description = "Ticket details for blog ticket 8", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityUrgent, TicketStatusId = statusDev,  TicketTypeId = typeNewDev},
				new Ticket() {Title = "Blog Ticket 9", Description = "Ticket details for blog ticket 9", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityLow, TicketStatusId = statusNew,  TicketTypeId = typeDefect},
				new Ticket() {Title = "Blog Ticket 10", Description = "Ticket details for blog ticket 10", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Blog Ticket 11", Description = "Ticket details for blog ticket 11", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev,  TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Blog Ticket 12", Description = "Ticket details for blog ticket 12", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew,  TicketTypeId = typeNewDev},
				new Ticket() {Title = "Blog Ticket 13", Description = "Ticket details for blog ticket 13", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeDefect},
				new Ticket() {Title = "Blog Ticket 14", Description = "Ticket details for blog ticket 14", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityMedium, TicketStatusId = statusDev,  TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Blog Ticket 15", Description = "Ticket details for blog ticket 15", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew,  TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Blog Ticket 16", Description = "Ticket details for blog ticket 16", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Blog Ticket 17", Description = "Ticket details for blog ticket 17", DateCreated = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev,  TicketTypeId = typeNewDev},
				//BUGTRACKER                                                                                                                         
				new Ticket() {Title = "Bug Tracker Ticket 1", Description = "Ticket details for bug tracker ticket 1", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 2", Description = "Ticket details for bug tracker ticket 2", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 3", Description = "Ticket details for bug tracker ticket 3", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 4", Description = "Ticket details for bug tracker ticket 4", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 5", Description = "Ticket details for bug tracker ticket 5", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 6", Description = "Ticket details for bug tracker ticket 6", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 7", Description = "Ticket details for bug tracker ticket 7", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 8", Description = "Ticket details for bug tracker ticket 8", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 9", Description = "Ticket details for bug tracker ticket 9", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 10", Description = "Ticket details for bug tracker 10", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 11", Description = "Ticket details for bug tracker 11", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 12", Description = "Ticket details for bug tracker 12", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 13", Description = "Ticket details for bug tracker 13", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 14", Description = "Ticket details for bug tracker 14", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 15", Description = "Ticket details for bug tracker 15", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 16", Description = "Ticket details for bug tracker 16", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 17", Description = "Ticket details for bug tracker 17", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 18", Description = "Ticket details for bug tracker 18", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 19", Description = "Ticket details for bug tracker 19", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 20", Description = "Ticket details for bug tracker 20", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 21", Description = "Ticket details for bug tracker 21", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 22", Description = "Ticket details for bug tracker 22", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 23", Description = "Ticket details for bug tracker 23", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 24", Description = "Ticket details for bug tracker 24", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 25", Description = "Ticket details for bug tracker 25", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 26", Description = "Ticket details for bug tracker 26", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 27", Description = "Ticket details for bug tracker 27", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 28", Description = "Ticket details for bug tracker 28", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 29", Description = "Ticket details for bug tracker 29", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Bug Tracker Ticket 30", Description = "Ticket details for bug tracker 30", DateCreated = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				//MOVIE
				new Ticket() {Title = "Movie Ticket 1", Description = "Ticket details for movie ticket 1", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeDefect},
				new Ticket() {Title = "Movie Ticket 2", Description = "Ticket details for movie ticket 2", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityMedium, TicketStatusId = statusDev, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Movie Ticket 3", Description = "Ticket details for movie ticket 3", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Movie Ticket 4", Description = "Ticket details for movie ticket 4", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Movie Ticket 5", Description = "Ticket details for movie ticket 5", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityLow, TicketStatusId = statusDev,  TicketTypeId = typeDefect},
				new Ticket() {Title = "Movie Ticket 6", Description = "Ticket details for movie ticket 6", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew,  TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Movie Ticket 7", Description = "Ticket details for movie ticket 7", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew, TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Movie Ticket 8", Description = "Ticket details for movie ticket 8", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityUrgent, TicketStatusId = statusDev,  TicketTypeId = typeNewDev},
				new Ticket() {Title = "Movie Ticket 9", Description = "Ticket details for movie ticket 9", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityLow, TicketStatusId = statusNew,  TicketTypeId = typeDefect},
				new Ticket() {Title = "Movie Ticket 10", Description = "Ticket details for movie ticket 10", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityMedium, TicketStatusId = statusNew, TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Movie Ticket 11", Description = "Ticket details for movie ticket 11", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev,  TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Movie Ticket 12", Description = "Ticket details for movie ticket 12", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew,  TicketTypeId = typeNewDev},
				new Ticket() {Title = "Movie Ticket 13", Description = "Ticket details for movie ticket 13", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityLow, TicketStatusId = statusNew, TicketTypeId = typeDefect},
				new Ticket() {Title = "Movie Ticket 14", Description = "Ticket details for movie ticket 14", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityMedium, TicketStatusId = statusDev,  TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Movie Ticket 15", Description = "Ticket details for movie ticket 15", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew,  TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Movie Ticket 16", Description = "Ticket details for movie ticket 16", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
				new Ticket() {Title = "Movie Ticket 17", Description = "Ticket details for movie ticket 17", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusDev,  TicketTypeId = typeNewDev},
				new Ticket() {Title = "Movie Ticket 18", Description = "Ticket details for movie ticket 18", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityMedium, TicketStatusId = statusDev,  TicketTypeId = typeEnhancement},
				new Ticket() {Title = "Movie Ticket 19", Description = "Ticket details for movie ticket 19", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityHigh, TicketStatusId = statusNew,  TicketTypeId = typeChangeRequest},
				new Ticket() {Title = "Movie Ticket 20", Description = "Ticket details for movie ticket 20", DateCreated = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = priorityUrgent, TicketStatusId = statusNew, TicketTypeId = typeNewDev},
			};

			var dbTickets = context.Tickets.Select(c => c.Title).ToList();
			await context.Tickets.AddRangeAsync(tickets.Where(c => !dbTickets.Contains(c.Title)));
			await context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine("*************  ERROR  *************");
			Console.WriteLine("Error Seeding Tickets.");
			Console.WriteLine(ex.Message);
			Console.WriteLine("***********************************");
			throw;
		}
	}
	#endregion
}