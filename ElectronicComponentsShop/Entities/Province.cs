using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Entities
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<District> Districts { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
