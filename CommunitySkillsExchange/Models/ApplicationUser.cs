using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CommunitySkillsExchange.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public decimal Rating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;

        // Navigation properties
        public virtual ICollection<SkillOffer> SkillOffers { get; set; }
        public virtual ICollection<SkillRequest> SkillRequests { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Review> ReviewsGiven { get; set; }
        public virtual ICollection<Review> ReviewsReceived { get; set; }
    }
}