﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAptitudeTest.Model
{
    public class Occupation
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        public virtual List<CateParts>? Cates { get; set; }
        public virtual User? user { get; set; } // of manager
        [ForeignKey(nameof(User.Id))]
        public string? userId { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        public List<Info>? infos { get; set; }
        public bool Active { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
