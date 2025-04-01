namespace LibraryDomain.Model;

public partial class Friendship
{
    public int Id { get; set; }

    public required string User1Id { get; set; }

    public required string User2Id { get; set; }

    public virtual User? User1 { get; set; }

    public virtual User? User2 { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }


}
