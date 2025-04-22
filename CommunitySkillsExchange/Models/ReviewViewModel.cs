using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.Models
{
    public class ReviewViewModel
    {
        public string ReviewedUserId { get; set; }
        public string ReviewedUserName { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }
    }
}