using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        public string UserName { get; set; }
        [Required]
        [StringLength(250)]
        public string Password { get; set; }
        [Required]
        public string Education { get; set; }
        [Required]
        public string Experience { get; set; }
        [Required]
        [StringLength(50)]
        [ForeignKey(nameof(Roles.Id))]
        public string RoleId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
       
    }
}
