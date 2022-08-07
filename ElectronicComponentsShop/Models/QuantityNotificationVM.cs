using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class QuantityNotificationVM
    {
        public string Title { get; set; }
        public string Quantity { get; set; }

        public QuantityNotificationVM() { }
        public QuantityNotificationVM(string title, string quantity)
        {
            Title = title;
            Quantity = quantity;
        }
    }
}
