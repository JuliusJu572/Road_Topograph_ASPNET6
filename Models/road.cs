using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class road
    {
        [Display(Name = "Road Name")]
        public string id { get; set; }

        [Display(Name = "Upper Facility")]
        public string? facility_id { get; set; }

        [Display(Name = "Road Type")]
        public string type { get; set; }

        [Display(Name = "Direction")]
        public string? direction { get; set; }

        [Display(Name = "Road Code")]
        public string? code { get; set; }

        [Display(Name = "Start Node")]
        public string? start_node { get; set; }

        [Display(Name = "Start Link Node")]
        public string? start_linknode { get; set; }

        [Display(Name = "End Node")]
        public string? end_node { get; set; }

        [Display(Name = "End Link Node")]
        public string? end_linknode { get; set; }

        [Display(Name = "Average Length")]
        public double? avg_length { get; set; }
    }
}
