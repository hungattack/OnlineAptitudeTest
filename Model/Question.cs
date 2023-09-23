using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class Question
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        public virtual CateParts? Cate { get; set; }
        [Required]
        [StringLength(50)]
        [ForeignKey(nameof(CateParts.Id))]
        public string PartId { get; set; }
        [Required]
        [StringLength(300)]
        public string? QuestionName { get; set; }
        [StringLength(300)]
        public string? AnswerArray { get; set; }
        [StringLength(300)]
        public string? Answer { get; set; }
        
        [Required]
        public int Point { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



    }
}
