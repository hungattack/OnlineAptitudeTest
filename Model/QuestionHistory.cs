using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class QuestionHistory
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [ForeignKey(nameof(User.Id))]
        public string userId { get; set; }
        public virtual Occupation? occupation { get; set; }
        [Required]
        [ForeignKey(nameof(Occupation.Id))]
        public string occupationId { get; set; }
        public int pointAll { get; set; }
        public List<ResultHistory>? Results { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
