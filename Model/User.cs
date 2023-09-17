using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class User
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        [StringLength(40)]
        public string? Email { get; set; }
        [Required]
        [StringLength(40)]
        public string? Name { get; set; }
        [Required]
        [StringLength(250)]
        public string? Password { get; set; }
       
        [StringLength(50)]
        [ForeignKey(nameof(Roles.Id))]
        public string? RoleId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
       
    }
}
