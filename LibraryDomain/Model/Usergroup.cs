﻿namespace LibraryDomain.Model;

public partial class UserGroup
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public int GroupId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual User? User { get; set; }
}
