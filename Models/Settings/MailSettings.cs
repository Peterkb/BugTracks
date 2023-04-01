namespace BugTracksV3.Models
{
    public class MailSettings
    {
        //Simple class with values (no id needed)
        public string Email { get; set; } = string.Empty;

        public string EmailDisplayName { get; set; } = string.Empty;

        public string EmailPassword { get; set; } = string.Empty;

        public string EmailHost { get; set; } = string.Empty;

        public int EmailPort { get; set; }
    }
}