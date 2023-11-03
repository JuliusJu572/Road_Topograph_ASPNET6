using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadAppWEB.Models
{
    public class node
    {
        [Display(Name = "Node Name")]
        public string id { get; set; }

        [Display(Name = "Hub Node")]
        public string? hubnode { get; set; }

        [Display(Name = "Father Node")]
        public string? fathernode { get; set; }

        [Display(Name = "Child Node")]
        public string? childnode { get; set; }

        [Display(Name = "Road Name")]
        public string? road_id { get; set; }

        [Display(Name = "Level")]
        public int level { get; set; }

        [Display(Name = "Longitude")]
        public double longitude { get; set; }

        [Display(Name = "Latitude")]
        public double latitude { get; set; }

        [Display(Name = "Span")]
        public double? span { get; set; }
    }
}
