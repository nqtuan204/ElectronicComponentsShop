using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class ColumnVM
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public ColumnVM(string key, string label)
        {
            Key = key;
            Label = label;
        }
    }
}
