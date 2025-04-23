using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CommunitySkillsExchange.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime JoinDate { get; set; }
        public int SkillsOffered { get; set; }
        public int SessionsCompleted { get; set; }
        public double AverageRating { get; set; }
    }

    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime JoinDate { get; set; }
        public IEnumerable<SkillViewModel> OfferedSkills { get; set; }
        public int SessionsCompleted { get; set; }
        public double AverageRating { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Bio { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }

        public string CurrentProfileImageUrl { get; set; }
    }
}