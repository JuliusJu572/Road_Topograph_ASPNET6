using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class ramp
    {
        [Display(Name = "Ramp Name")]
        public string id { get; set; }

        [Display(Name = "Upper Viaduct")]
        public string? viaduct_id { get; set; }

        [Display(Name = "Upper Overpass")]
        public string? overpass_id { get; set; }

        [Display(Name = "Direction")]
        public string direction { get; set; }

        [Display(Name = "Road Code")]
        public string code { get; set; }

        [Display(Name = "Start Node")]
        public string? start_node { get; set; }

        [Display(Name = "End Node")]
        public string? end_node { get; set; }

        [Display(Name = "Link Node")]
        public string? linknode { get; set; }

        [Display(Name = "Average Length")]
        public float avg_length { get; set; }
    }
}
