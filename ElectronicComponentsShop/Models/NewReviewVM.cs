using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class NewReviewVM
    {
        public string Content { get; set; }
        public int Score { get; set; }
        public int ProductId { get; set; }
    }
}
