using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
