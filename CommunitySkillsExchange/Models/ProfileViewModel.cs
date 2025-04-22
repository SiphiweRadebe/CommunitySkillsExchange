namespace CommunitySkillsExchange.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public List<SkillOffer> SkillOffers { get; set; }
        public List<SkillRequest> SkillRequests { get; set; }
        public List<Review> Reviews { get; set; }
    }
}