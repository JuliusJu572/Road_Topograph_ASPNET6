using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class mainline
    {
        [Display(Name = "Mainline Name")]
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
        public string? StartNode { get; set; }

        [Display(Name = "End Node")]
        public string? EndNode { get; set; }

        [Display(Name = "Average Length")]
        public float avg_length { get; set; }
    }
}
