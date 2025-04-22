using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        // Navigation properties
        public virtual Conversation Conversation { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}