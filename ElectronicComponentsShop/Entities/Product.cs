using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Models;
namespace ElectronicComponentsShop.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public int Views { get; set; }
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

        public Product() {  }
        public Product(NewProduct product)
        {
            Name = product.Name;
            Price = product.Price;
            Slug = GetSlug(product.Name);
            ThumbnailURL = product.ThumbnailURL;
            CategoryId = product.CategoryId;
        }

        private string GetSlug(string s)
        {
            System.Text.RegularExpressions.Regex regex = new(@"[^\d\w]");
            System.Text.RegularExpressions.Regex regex1 = new("[àáảãạăằắẳẵặâầấẩẫậ]");
            System.Text.RegularExpressions.Regex regex2 = new("[èéẻẽẹêềếểễệ]");
            System.Text.RegularExpressions.Regex regex3 = new("[ìíỉĩị]");
            System.Text.RegularExpressions.Regex regex4 = new("òóỏõọôồốổỗộơờớởỡợ");
            System.Text.RegularExpressions.Regex regex5 = new("ùúủũụưừứửữự");
            Slug = s.ToLower();
            Slug = regex1.Replace(Slug, "a");
            Slug = regex2.Replace(Slug, "e");
            Slug = regex3.Replace(Slug, "i");
            Slug = regex4.Replace(Slug, "o");
            Slug = regex5.Replace(Slug, "u");
            Slug = regex.Replace(Slug, "-");
            return Slug;
        }
    }
}
