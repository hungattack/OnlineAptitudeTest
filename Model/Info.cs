using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class Info
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [ForeignKey(nameof(User.Id))]
        public string managerId { get; set; }
        [ForeignKey(nameof(Occupation.Id))]
        public string occupationId { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Company { get; set; }
        [StringLength(50)]
        public string Contact { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Introduction { get; set; }
        [StringLength(500)]
        public string Requirement { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
