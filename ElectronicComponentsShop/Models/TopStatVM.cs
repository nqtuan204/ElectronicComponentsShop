using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class TopStatVM
    {
        public string Title { get; set; }
        public IDictionary<string, decimal> TopList { get; set; }
    }
}
