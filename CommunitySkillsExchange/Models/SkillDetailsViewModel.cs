using CommunitySkillsExchange.Models;
using System.Collections.Generic;

namespace CommunitySkillsExchange.ViewModels
{
    public class SkillDetailsViewModel
    {
        public SkillDetailModel Skill { get; set; }
        public UserProfileViewModel User { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public bool UserHasCompletedSession { get; set; }
    }

    public class SkillDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ExperienceLevel { get; set; }
        public string TeachingMethod { get; set; }
        public string MaterialsRequired { get; set; }
        public string Availability { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
}