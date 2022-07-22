using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }

        public ReviewDTO(Review review)
        {
            Id = review.Id;
            Score = review.Score;
            Content = review.Content;
            CreatedAt = review.CreatedAt;
            if (review.User == null)
                UserName = "Ẩn danh";
            else
                UserName = review.User.LastName + " " + review.User.FirstName;
        }
    }
}
