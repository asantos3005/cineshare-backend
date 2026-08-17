namespace cineshare_backend.Models;

public class Like
{
    public int UserId { get; set; }
    public User LikedBy { get; set; } = null!;

    public int ReviewId { get; set; }
    public Review LikedReview { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/*
A particular user-review combination can exist only once. 
Use the composite key because Like really represents the relationship itself 
don't currently need to refer to a like independently by something like /likes/437.
*/