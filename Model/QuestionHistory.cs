using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class QuestionHistory
    {
        [Key]
        [StringLength(50)]

        public string UserId { get; set; }
        public virtual Question? question { get; set; }
        [Required]
        [ForeignKey(nameof(Question.Id))]
        public string QuestionId { get; set; }
        [Required]
        [StringLength(300)]
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
