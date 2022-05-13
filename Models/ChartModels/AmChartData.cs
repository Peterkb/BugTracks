namespace BugTracksV3.Models.ChartModels
{
    public class AmChartData
    {
        public AmItem[]? Data { get; set; }
    }


    public class AmItem
    {
        public string Project { get; set; } = string.Empty;
        public int Tickets { get; set; }
        public int Developers { get; set; }
    }
}