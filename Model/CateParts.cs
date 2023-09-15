using System.ComponentModel.DataAnnotations;

namespace OnlineAptitudeTest.Model
{
    public class CateParts
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
