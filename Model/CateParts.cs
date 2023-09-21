using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class CateParts
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        public virtual User? user { get; set; }
        [StringLength(50)]
        [ForeignKey(nameof(User.Id))]
        public string userId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int TimeOut { get; set; }
        [Required]
        [StringLength(10)]
        public string TimeType { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
