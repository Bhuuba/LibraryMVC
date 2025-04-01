using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryDomain.Model;

public partial class Post : Entity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Coment> Coments { get; set; } = new List<Coment>();

    public virtual User? User { get; set; }

}
