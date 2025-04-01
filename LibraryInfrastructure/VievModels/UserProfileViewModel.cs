using System.Collections.Generic;
using LibraryDomain.Model;

namespace LibraryInfrastructure.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<User> Friends { get; set; } = new List<User>();
    }
}
