using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class User
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        public bool Gender { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string? Password { get; set; }
        public virtual Roles? roles { get; set; }
        [StringLength(50)]
        [ForeignKey(nameof(Roles.Id))]
        public string? RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
