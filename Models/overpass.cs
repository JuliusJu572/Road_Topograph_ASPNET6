using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class overpass
    {
        [Display(Name = "Overpass Name")]
        public string id { get; set; }

        [Display(Name = "Trunk Network")]
        public string? trunknetwork { get; set; }

        [Display(Name = "Maintenance Unit")]
        public string? maintenanceunit { get; set; }

        [Display(Name = "Operating Unit")]
        public string? operatingunit { get; set; }

        [Display(Name = "Line of Lanes")]
        public int lanes { get; set; }

        [Display(Name = "Length")]
        public double? length { get; set; }

        [Display(Name = "Square")]
        public double? square { get; set; }

        [Display(Name = "Date of Completion")]
        public string? date { get; set; }
    }
}
