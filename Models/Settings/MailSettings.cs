namespace BugTracksV3.Models
{
    public class MailSettings
    {
        //Simple class with values (no id needed)
        public string Mail { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }
    }
}