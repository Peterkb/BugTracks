namespace BugTracksV3.Models.ChartModels
{
    public class PlotlyBarData
    {
        public List<PlotlyBar>? Data { get; set; }
    }


    public class PlotlyBar
    {
        public string[]? X { get; set; }
        public int[]? Y { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
