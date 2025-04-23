using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string SkillTitle { get; set; }
        public string SkillOwnerName { get; set; }
        public string SkillOwnerUserId { get; set; }
        public string RequestorName { get; set; }
        public string RequestorUserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class CreateRequestViewModel
    {
        public int SkillId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }
    }

    public class UpdateRequestViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Status { get; set; }

        public string ResponseMessage { get; set; }
    }
}