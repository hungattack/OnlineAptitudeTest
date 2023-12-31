﻿using System.ComponentModel.DataAnnotations;

namespace OnlineAptitudeTest.Model
{
    public class Roles
    {
        [Key]
        [StringLength(50)]
        public string? Id { get; set; }
        [StringLength(20)]

        public string? Name { get; set; }
        [StringLength(300)]

        public string? Description { get; set; }
        [StringLength(50)]

        public string? Permissions { get; set; }

    }
}
