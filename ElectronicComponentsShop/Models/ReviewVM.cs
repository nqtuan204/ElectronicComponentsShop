using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Models
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }

        public ReviewVM(ReviewDTO dto)
        {
            Id = dto.Id;
            Score = dto.Score;
            Content = dto.Content;
            CreatedAt = dto.CreatedAt;
            UserName = dto.UserName;
        }
    }
}
