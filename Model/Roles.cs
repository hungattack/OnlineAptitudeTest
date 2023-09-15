using System.ComponentModel.DataAnnotations;

namespace OnlineAptitudeTest.Model
{
    public class Roles
    {
        [Key]
        public string Id {  get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Permissions { get; set; }

    }
}
