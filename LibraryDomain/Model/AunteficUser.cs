using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDomain.Model
{
    class AunteficUser: IdentityUser
    {
        public int Year { get; set; }
    }
}
