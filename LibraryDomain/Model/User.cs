
using Microsoft.AspNetCore.Identity;

namespace LibraryDomain.Model
{
    public partial class User : IdentityUser
    {
        public DateOnly? Birthdate { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Coments { get; set; } = new List<Comment>();
        public virtual ICollection<Friendship> FriendshipUser1s { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> FriendshipUser2s { get; set; } = new List<Friendship>();
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
