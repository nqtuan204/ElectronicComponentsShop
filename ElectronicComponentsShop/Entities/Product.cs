using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        public string ThumbnailURL { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<ProductImage> Images { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<Favourite> Favourites { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
