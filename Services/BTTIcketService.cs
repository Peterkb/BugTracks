using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Models;
using BugTracksV3.Models.Enums;
using BugTracksV3.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTracksV3.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context, IBTRolesService rolesService, IBTProjectService projectService)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }

        //Create
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            try
            {
                await _context.AddAsync(ticketAttachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Add Ticket Comment
        public async Task AddTicketCommentAsync(TicketComment ticketComment)
        {
            try
            {
                await _context.AddAsync(ticketComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //Archive
        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            try
            {
                //Restore ticket if it is archived and visa versa
                if (ticket.IsArchived)
                {
                    ticket.IsArchived = false;
                }
                else
                {
                    ticket.IsArchived = true;
                }

                await UpdateTicketAsync(ticket);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            Ticket ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            try
            {
                if (ticket != null)
                {
                    try
                    {
                        ticket.DeveloperUserId = userId;
                        //Revisit this code when assigning Tickets
                        ticket.TicketStatusId = (await LookupTicketStatusIdAsync("Development")).Value;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                        .Include(p => p.Attachments)
                                                        .Include(p => p.Comments)
                                                        .Include(p => p.DeveloperUser)
                                                        .Include(p => p.History)
                                                        .Include(p => p.TicketPriority)
                                                        .Include(p => p.TicketStatus)
                                                        .Include(p => p.TicketType)
                                                        .Include(p => p.Project)
                                                     .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            //return (await GetAllTicketsByCompanyAsync(companyId)).Where(p => p.TicketPriority.Name == priorityName).ToList();

            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                        .Include(p => p.Attachments)
                                                        .Include(p => p.Comments)
                                                        .Include(p => p.DeveloperUser)
                                                        .Include(p => p.History)
                                                        .Include(p => p.TicketPriority)
                                                        .Include(p => p.TicketStatus)
                                                        .Include(p => p.TicketType)
                                                        .Include(p => p.Project)
                                                     .Where(t => t.TicketPriorityId == priorityId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            int statusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                        .Include(p => p.Attachments)
                                                        .Include(p => p.Comments)
                                                        .Include(p => p.DeveloperUser)
                                                        .Include(p => p.History)
                                                        .Include(p => p.TicketPriority)
                                                        .Include(p => p.TicketStatus)
                                                        .Include(p => p.TicketType)
                                                        .Include(p => p.Project)
                                                     .Where(t => t.TicketStatusId == statusId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int typeId = (await LookupTicketStatusIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                        .Include(p => p.Attachments)
                                                        .Include(p => p.Comments)
                                                        .Include(p => p.DeveloperUser)
                                                        .Include(p => p.History)
                                                        .Include(p => p.TicketPriority)
                                                        .Include(p => p.TicketStatus)
                                                        .Include(p => p.TicketType)
                                                        .Include(p => p.Project)
                                                     .Where(t => t.TicketTypeId == typeId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.IsArchived == true).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByPriorityAsync(companyId, priorityName)).Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            List<Ticket> tickets = new();
            try
            {
                tickets = (await GetTicketsByRoleAsync(role, userId, companyId)).Where(t => t.ProjectId == projectId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return tickets;
        }

        #region Get Project Tickets By Status
        public async Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByStatusAsync(companyId, statusName)).Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Project Tickets By Type
        public async Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByTypeAsync(companyId, typeName)).Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Ticket Attachment By Id
        public async Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId)
        {
            try
            {
                TicketAttachment ticketAttachment = await _context.TicketAttachments
                                                                  .Include(t => t.User)
                                                                  .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
                return ticketAttachment;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Ticket As No Tracking
        public async Task<Ticket> GetTicketAsNoTrackingAsync(int ticketId)
        {
            try
            {
                return await _context.Tickets
                                    .Include(t => t.DeveloperUser)
                                    .Include(t => t.Project)
                                    .Include(t => t.TicketPriority)
                                    .Include(t => t.TicketStatus)
                                    .Include(t => t.TicketType)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        //Get Ticket
        #region Get Ticket By Id
        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                return await _context.Tickets
                                    .Include(t => t.DeveloperUser)
                                    .Include(t => t.OwnerUser)
                                    .Include(t => t.Project)
                                    .Include(t => t.TicketPriority)
                                    .Include(t => t.TicketStatus)
                                    .Include(t => t.TicketType)
                                    .Include(t => t.Comments)
                                    .Include(t => t.Attachments)
                                    .Include(t => t.History)
                                    .FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Ticket Developer
        public async Task<ApplicationUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            ApplicationUser developer = new();

            try
            {
                Ticket ticket = (await GetAllTicketsByCompanyAsync(companyId)).FirstOrDefault(t => t.Id == ticketId);
                if (ticket?.DeveloperUserId != null)
                {
                    developer = ticket.DeveloperUser;
                }
                return developer;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            List<Ticket> tickets = new();
            try
            {
                if (role == Roles.Admin.ToString())
                {
                    tickets = await GetAllTicketsByCompanyAsync(companyId);
                }
                else if (role == Roles.Developer.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.DeveloperUserId == userId).ToList();
                }
                else if (role == Roles.Submitter.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.OwnerUserId == userId).ToList();
                }
                else if (role == Roles.ProjectManager.ToString())
                {
                    tickets = await GetTicketsByUserIdAsync(userId, companyId);
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Ticket> tickets = new();
            try
            {
                if (await _rolesService.IsUserInRoleAsync(user, Roles.Admin.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId)).SelectMany(t => t.Tickets).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.Developer.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(t => t.Tickets)
                                                    .Where(t => t.DeveloperUserId == userId)
                                                    .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(t => t.Tickets)
                                                    .Where(t => t.OwnerUserId == userId)
                                                    .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(user, Roles.ProjectManager.ToString()))
                {
                    tickets = (await _projectService.GetUserProjectsAsync(userId))
                                                    .SelectMany(t => t.Tickets)
                                                    .ToList();
                }
                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Get Unassigned Tickets
        public async Task<List<Ticket>> GetUnassignedTicketsAsync(int companyId)
        {
            try
            {
                //Return all tickets without developers
                List<Ticket> tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => string.IsNullOrEmpty(t.DeveloperUserId)).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                TicketPriority priority = await _context.TicketPriorities.FirstOrDefaultAsync(p => p.Name == priorityName);
                return priority?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus status = await _context.TicketStatuses.FirstOrDefaultAsync(p => p.Name == statusName);
                return status?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType type = await _context.TicketTypes.FirstOrDefaultAsync(p => p.Name == typeName);
                return type?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Update
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
