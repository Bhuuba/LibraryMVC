namespace LibraryDomain.Model;

public partial class Post
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public virtual User? User { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();



}
