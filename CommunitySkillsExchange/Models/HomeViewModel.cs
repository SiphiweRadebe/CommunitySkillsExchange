namespace CommunitySkillsExchange.Models
{
    public class HomeViewModel
    {
        public List<SkillOffer> RecentOffers { get; set; }
        public List<SkillRequest> RecentRequests { get; set; }
        public List<SkillCategory> Categories { get; set; }
    }
}