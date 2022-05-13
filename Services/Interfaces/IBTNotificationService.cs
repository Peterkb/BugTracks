using BugTracksV3.Areas.Identity.Data;
using BugTracksV3.Models;

namespace BugTracksV3.Services.Interfaces
{
    public interface IBTNotificationService
    {
        public Task AddNotificationAsync(Notification notification);

        public Task<List<Notification>> GetReceivedNotificationsAsync(string userId);

        public Task<List<Notification>> GetSentNotificationsAsync(string userId);

        public Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);

        //List notifications to list of members
        public Task SendMembersEmailNotificationsAsync(Notification notification, List<ApplicationUser> members);

        public Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject);
    }
}
