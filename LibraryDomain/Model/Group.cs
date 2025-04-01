namespace LibraryDomain.Model;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Info { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
