namespace CommunitySkillsExchange.Models
{
    public class AdminDashboardViewModel
    {
        public int UserCount { get; set; }
        public int SkillOffersCount { get; set; }
        public int SkillRequestsCount { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; }
        public List<SkillOffer> RecentOffers { get; set; }
        public List<SkillRequest> RecentRequests { get; set; }
    }
}