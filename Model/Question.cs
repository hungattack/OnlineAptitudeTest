using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class Question
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        [ForeignKey(nameof(CateParts.Id))]
        public string PartId { get; set; }
        [Required]
        [StringLength(300)]
        public string QuestionName { get; set; }
        [Required]
        [StringLength(300)]
        public string Answer { get; set; }
        [Required]
        [MaxLength(5)]
        public int Point { get; set; }
        [Required]
        [MaxLength(11)]
        public int TimeOut { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



    }
}
