using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Friendship : Entity
{
    public int Id { get; set; }

    public int User1Id { get; set; }

    public int User2Id { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual User? User1 { get; set; }

    public virtual User? User2 { get; set; }
}
