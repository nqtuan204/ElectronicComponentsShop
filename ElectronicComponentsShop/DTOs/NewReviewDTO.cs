using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.DTOs
{
    public class NewReviewDTO
    {
        public int? UserId { get; set; }
        public string Content { get; set; }
        public int Score { get; set; }
        public int ProductId { get; set; }

        public NewReviewDTO(NewReviewVM newReview, int? userId)
        {
            UserId = userId;
            Content = newReview.Content;
            Score = newReview.Score;
            ProductId = newReview.ProductId;
        }
    }
}
