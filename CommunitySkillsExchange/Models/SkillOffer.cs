using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.Models
{
    public class SkillOffer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Range(-90, 90)]
        public decimal? Latitude { get; set; }

        [Range(-180, 180)]
        public decimal? Longitude { get; set; }

        [StringLength(200)]
        public string LocationDescription { get; set; }

        // Navigation properties
        public virtual SkillCategory Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}