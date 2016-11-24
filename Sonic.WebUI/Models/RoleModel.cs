using System.Collections.Generic;
using Sonic.Domain.Entities;

namespace Sonic.WebUI.Models
{
    public class RoleModel
    {
        public Domain.Entities.System System { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}