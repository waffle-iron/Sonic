﻿using System.ComponentModel.DataAnnotations;

namespace Sonic.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        public int SystemId { get; set; }
        public System System { get; set; }
    }
}
