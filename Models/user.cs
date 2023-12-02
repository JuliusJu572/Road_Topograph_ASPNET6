using System.ComponentModel.DataAnnotations;

namespace RoadAppWEB.Models
{
    public class user
    {
        [Display(Name = "User ID")]
        public int id { get; set; }

        [Display(Name = "User Name")]
        public string name { get; set; }

        [Display(Name = "User Password")]
        public string password { get; set; }

        [Display(Name = "Authority")]
        public string authority { get; set; }
    }
}
