using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Entity
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
