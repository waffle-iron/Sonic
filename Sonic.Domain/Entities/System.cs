using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sonic.Domain.Entities
{
    public class System
    {
        public int SystemId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
