using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateReviewViewModel
    {
        public int SkillId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }
    }
}