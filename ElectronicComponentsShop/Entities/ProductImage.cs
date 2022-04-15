using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
