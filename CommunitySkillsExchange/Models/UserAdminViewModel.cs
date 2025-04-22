namespace CommunitySkillsExchange.Models
{
    public class UserAdminViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}