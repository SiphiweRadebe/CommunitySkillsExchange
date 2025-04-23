using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string SenderName { get; set; }
        public string RecipientUserId { get; set; }
        public string RecipientName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }

    public class MessageThreadViewModel
    {
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string OtherUserImageUrl { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }

    public class MessageListViewModel
    {
        public List<MessageThreadSummary> Threads { get; set; }
        public int UnreadCount { get; set; }
    }

    public class MessageThreadSummary
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string LastMessagePreview { get; set; }
        public DateTime LastMessageDate { get; set; }
        public bool HasUnread { get; set; }
    }

    public class SendMessageViewModel
    {
        public string RecipientUserId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}