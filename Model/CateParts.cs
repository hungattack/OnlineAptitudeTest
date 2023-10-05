using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class CateParts
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        public virtual Occupation? occupation { get; set; }
        [Required]
        [ForeignKey(nameof(Occupation.Id))]
        public string OccupationId { get; set; }
        public virtual List<Question>? Questions { get; set; }
        public string? Name { get; set; }
        public int? TimeOut { get; set; }

        [StringLength(10)]
        public string? TimeType { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
