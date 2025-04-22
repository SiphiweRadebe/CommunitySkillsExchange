using System;
using System.Collections.Generic;

namespace CommunitySkillsExchange.Models
{
    public class SkillCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<SkillOffer> SkillOffers { get; set; }
        public virtual ICollection<SkillRequest> SkillRequests { get; set; }
    }
}