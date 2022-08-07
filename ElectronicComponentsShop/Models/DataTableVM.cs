using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class DataTableVM<T>
    {
        public IList<ColumnVM> Columns { get; set; }
        public IList<T> Data { get; set; }
    }
}
