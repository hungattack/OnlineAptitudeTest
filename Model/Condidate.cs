using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class Condidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual User? user { get; set; }
        public virtual Occupation? occupation { get; set; }
        [ForeignKey(nameof(User.Id))]
        public string? managerId { get; set; }
        [ForeignKey(nameof(Occupation.Id))]
        public string? occupationId { get; set; }
        [ForeignKey(nameof(User.Id))]
        public string? userId { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(40)]
        public string? UserName { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        [StringLength(250)]
        public string? Password { get; set; }
        [StringLength(250)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? BirthDay { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public bool Start { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
