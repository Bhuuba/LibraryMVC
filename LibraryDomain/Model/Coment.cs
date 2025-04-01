using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Coment : Entity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
