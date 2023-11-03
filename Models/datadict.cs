using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class datadict
    {
        [Display(Name = "Index")]
        public int id { get; set; }

        [Display(Name = "Reference")]
        public string? reference { get; set; }

        [Display(Name = "Content")]
        public string? content { get; set; }
    }
}
