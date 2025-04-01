using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Group : Entity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Info { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Usergroup> Usergroups { get; set; } = new List<Usergroup>();
}
