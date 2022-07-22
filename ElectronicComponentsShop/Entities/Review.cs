using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Review() { }

        public Review(NewReviewDTO dto)
        {
            Score = dto.Score;
            Content = dto.Content;
            CreatedAt = DateTime.Now;
            UserId = dto.UserId;
            ProductId = dto.ProductId;
        }
    }
}
