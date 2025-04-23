using System;
using System.ComponentModel.DataAnnotations;

namespace CommunitySkillsExchange.ViewModels
{
    public class SkillViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public double AverageRating { get; set; }

        public int TotalRatings { get; set; }
    }

    public class SkillCreateViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Experience Level")]
        public string ExperienceLevel { get; set; }

        [Required]
        [Display(Name = "Teaching Method")]
        public string TeachingMethod { get; set; }

        [Display(Name = "Materials Required")]
        public string MaterialsRequired { get; set; }

        [Display(Name = "Availability")]
        public string Availability { get; set; }
    }

    public class SkillEditViewModel : SkillCreateViewModel
    {
        public int Id { get; set; }
    }
}