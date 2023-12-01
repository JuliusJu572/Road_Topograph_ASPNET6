namespace RoadAppWEB.Models
{
    public class HubNodeRes
    {
        public int id { get; set; }
        public string? starthub_id { get; set; }
        public string? endhub_id { get; set; }
        public double? span { get; set; }
        public string? velocity { get; set; }
        public string? direction { get; set; }
        public string? type { get; set; }
    }
}
