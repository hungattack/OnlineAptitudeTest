using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class ResultHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(QuestionHistory.occupationId))]
        public string occupaionId { get; set; }
        public virtual CateParts? catePart { get; set; }
        [ForeignKey(nameof(QuestionHistory.Id))]
        public string questionHisId { get; set; }
        [ForeignKey(nameof(CateParts.Id))]
        public string catePartId { get; set; }
        [ForeignKey(nameof(Question.Id))]
        public string? questionId { get; set; }
        [StringLength(500)]
        public string? Answer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
