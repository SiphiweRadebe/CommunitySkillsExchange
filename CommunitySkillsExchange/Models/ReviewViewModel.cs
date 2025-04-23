using System.ComponentModel.DataAnnotations;

public class ReviewViewModel
{
    public int Id { get; set; }

    public string ReviewerId { get; set; }

    public string ReviewedUserId { get; set; }

    public string ReviewedUserName { get; set; } // Needed in AddReview GET action

    public int SkillId { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string Comment { get; set; }

    public DateTime CreatedDate { get; set; }
}
