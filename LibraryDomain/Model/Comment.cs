namespace LibraryDomain.Model;

public partial class Comment
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public virtual User? User { get; set; }

    public int PostId { get; set; }

    public virtual Post? Post { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    
}
