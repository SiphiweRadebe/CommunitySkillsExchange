using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string ReviewerId { get; set; }

        [Required]
        public string ReviewedUserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ApplicationUser Reviewer { get; set; }
        public virtual ApplicationUser ReviewedUser { get; set; }
    }
}